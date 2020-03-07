//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using LiveCharts;
    using LiveCharts.Defaults;
    using LiveCharts.Wpf;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PetrolUsageGraphViewModel : BindableBase, IDisposable
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IVehicleService _vehicleService;
        private ObservableCollection<ChartData> _expenses;
        private VehicleViewModel _selectedVehicle;
        private DateTime _fromDate;
        private DateTime _toDate;

        #endregion

        #region Properties
       
        public SeriesCollection SeriesCollection { get; set; }
        #endregion

        #region Methods
        public PetrolUsageGraphViewModel(IDataService dataService, IEventAggregator eventAggregator, IVehicleService vehicleService)
        {
            SeriesCollection = new SeriesCollection();
            _dataService = dataService;
            _vehicleService = vehicleService;

            ResetParameters();

            //eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
            eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>().Subscribe(OnFilteredDatesChanged);
        }

        private void OnFilteredDatesChanged(Tuple<DateTime, DateTime> updatedDates)
        {
            _fromDate = updatedDates.Item1;
            _toDate = updatedDates.Item2;
            FilterExpenses();
        }

        private void ResetParameters()
        {
            _expenses = new ObservableCollection<ChartData>();
            _fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _toDate = DateTime.Now;
        }

        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            ResetParameters();

            if (vehicle == null)
            {
                _selectedVehicle = null;
                return;
            }

            var model = new VehicleViewModel(_dataService, ServiceLocator.Current.GetInstance<INotificationsService>());
            model.LoadInstance(vehicle);
            _selectedVehicle = model;
            GetSelectedVehiclePetrolExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            if (_selectedVehicle == null)
                return;

            var expenses = _dataService.GetPetrolExpensesByVehicleId(_selectedVehicle.Vehicle.Id);

            if (expenses?.Result == null)
                return;

            foreach (var expense in expenses.Result)
            {
                _expenses.Add(new ChartData
                {
                    Cost = Convert.ToDouble(expense.Amount),
                    Date = expense.DateEntered.Value.ToShortDateString()
                });
            }

            FilterExpenses();
        }

        private void FilterExpenses()
        {
            SeriesCollection.Clear();

            if (_expenses == null) 
                return;

            foreach (var chartData in _expenses.Where(o => Convert.ToDateTime(o.Date).Date >= _fromDate
                                                           && Convert.ToDateTime(o.Date).Date <= _toDate)
                                                           .OrderBy(o => Convert.ToDateTime(o.Date).Date))
            {
                var series = new PieSeries
                {
                    Title = string.Empty,
                    Values = new ChartValues<ObservableValue> {new ObservableValue(chartData.Cost)},
                    DataLabels = true,
                };
                SeriesCollection.Add(series);
            }
        }
        #endregion

        public void Dispose()
        {
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
        }
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 