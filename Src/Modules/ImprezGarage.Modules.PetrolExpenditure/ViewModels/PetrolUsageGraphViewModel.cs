//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using LiveCharts;
    using LiveCharts.Defaults;
    using LiveCharts.Wpf;
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
        private Vehicle _selectedVehicle;
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

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
            eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>().Subscribe(OnFilteredDatesChanged);
            eventAggregator.GetEvent<PetrolEvents.ExpenseDeleted>().Subscribe(OnExpenseDeleted);
        }

        private void ResetParameters()
        {
            _expenses = new ObservableCollection<ChartData>();
            _fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _toDate = DateTime.Now;
        }

        private void OnFilteredDatesChanged(Tuple<DateTime, DateTime> updatedDates)
        {
            _fromDate = updatedDates.Item1;
            _toDate = updatedDates.Item2;
            GetSelectedVehiclePetrolExpenses();
            FilterExpenses();
        }


        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            ResetParameters();

            if (vehicle == null)
            {
                _selectedVehicle = null;
                return;
            }

            _selectedVehicle = vehicle;
            GetSelectedVehiclePetrolExpenses();
            FilterExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            _expenses.Clear();

            if (_selectedVehicle == null)
                return;

            var expenses = _dataService.GetPetrolExpensesByVehicleId(_selectedVehicle.Id);

            if (expenses?.Result == null)
                return;

            foreach (var expense in expenses.Result)
            {
                _expenses.Add(new ChartData
                {
                    Id = expense.Id,
                    Cost = Convert.ToDouble(expense.Amount),
                    Date = expense.DateEntered.Value.ToShortDateString()
                });
            }
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

        public void Dispose()
        {
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
        }

        public void OnExpenseDeleted(int id)
        {
            _expenses.Remove(_expenses.FirstOrDefault(o => o.Id.Equals(id)));
            FilterExpenses();
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 