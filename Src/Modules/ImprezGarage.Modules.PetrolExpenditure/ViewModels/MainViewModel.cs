//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Infrastructure;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using Views;


    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

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
        public MainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
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

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 