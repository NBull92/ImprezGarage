//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using Views;


    public class MainViewModel : BindableBase, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IVehicleService _vehicleService;

        #region Attributes
        private VehicleViewModel _selectedVehicle;
        #endregion

        #region Properties
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
                RaiseFilteredDatesChanged();
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
                RaiseFilteredDatesChanged();
            }
        }

        public DelegateCommand AddExpenditureCommand { get;set; }
        #endregion

        #region Methods
        public MainViewModel(IEventAggregator eventAggregator, IVehicleService vehicleService)
        {
            _eventAggregator = eventAggregator;
            _vehicleService = vehicleService;

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
            AddExpenditureCommand = new DelegateCommand(AddExpenditureExecute);
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = DateTime.Now;
        }

        #region Command Handlers

        private void AddExpenditureExecute()
        {
            var addExpense = new AddPetrolExpenditure();
            if (addExpense.DataContext is AddPetrolExpenditureViewModel vm)
                vm.VehicleId = _selectedVehicle.Vehicle.Id;
            addExpense.ShowDialog();
        }
        #endregion

        private void RaiseFilteredDatesChanged()
        {
            _eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>()
                .Publish(new Tuple<DateTime, DateTime>(FromDate, ToDate));
        }

        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            if (vehicle == null)
            {
                SelectedVehicle = null;
                return;
            }

            var model = new VehicleViewModel(ServiceLocator.Current.GetInstance<IDataService>(), ServiceLocator.Current.GetInstance<INotificationsService>());
            model.LoadInstance(vehicle);
            SelectedVehicle = model;
        }
        #endregion

        public void Dispose()
        {
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
        }
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 