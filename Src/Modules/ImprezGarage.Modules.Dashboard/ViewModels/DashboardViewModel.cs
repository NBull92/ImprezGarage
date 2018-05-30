//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Dashboard.ViewModels
{
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using ImprezGarage.Modules.MyGarage.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Collections.ObjectModel;

    public class DashboardViewModel : BindableBase
    {
        private DataService _dataService;
        private DialogService _dialogService;
        private IRegionManager _regionManager;

        public DelegateCommand<object> AddNewVehicleCommand { get; set; }

        private ObservableCollection<VehicleViewModel> _vehicles;
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set
            {
                SetProperty(ref _vehicles, value);
                RaisePropertyChanged("Vehicles");
            }
        }

        public DashboardViewModel(DataService dataService, DialogService dialogService, IRegionManager regionManager)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _regionManager = regionManager;

            AddNewVehicleCommand = new DelegateCommand<object>(AddNewVehicleExecute);

            LoadVehicles();
        }

        /// <summary>
        /// Retrieves all of the vehicles saved to the database and create a view model for each one.
        /// </summary>
        private void LoadVehicles()
        {
            Vehicles = new ObservableCollection<VehicleViewModel>();

            foreach (var vehicle in _dataService.GetVehicles())
            {
                var viewModel = new VehicleViewModel(_dialogService, _dataService);
                viewModel.LoadInstance(vehicle);
                Vehicles.Add(viewModel);
            }
        }

        #region Command Executes
        private void AddNewVehicleExecute(object navigationPath)
        {
            AddVehicle view = new AddVehicle();
            var window = _dialogService.ShowWindow(view, 250, 285);
            window.Closed += (sender, args) => { LoadVehicles(); };
        }
        #endregion
    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 