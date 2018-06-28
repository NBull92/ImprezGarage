//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.ViewModels;
    using Prism.Events;
    using Prism.Mvvm;

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
        #endregion

        #region Methods
        public MainViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 