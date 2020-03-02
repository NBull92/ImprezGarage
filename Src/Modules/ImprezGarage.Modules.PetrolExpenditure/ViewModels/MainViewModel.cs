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
    using Views;


    public class MainViewModel : BindableBase
    {
        #region Attributes
        private VehicleViewModel _selectedVehicle;
        #endregion

        #region Properties
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public DelegateCommand AddExpenditureCommand { get;set; }
        #endregion

        #region Methods
        public MainViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
            AddExpenditureCommand = new DelegateCommand(AddExpenditureExecute);
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

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 