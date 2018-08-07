//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Infrastructure.ViewModels;
    using ImprezGarage.Modules.PerformChecks.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Collections.ObjectModel;

    public class MainViewModel : BindableBase, INavigationAware
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly INotificationsService _notificationsService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private const string START_MAINTENANCE_CHECK_ERROR = "An error occured when attempting to start a maintenance check.";
        private const string NOTIFICATION_HEADER = "Alert!";

        private VehicleViewModel _selectedVehicle;
        private ObservableCollection<MaintenanceCheck> _vehicleMaintenanceChecksPerformed;
        #endregion

        #region Properties
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public ObservableCollection<MaintenanceCheck> VehicleMaintenanceChecksPerformed
        {
            get => _vehicleMaintenanceChecksPerformed;
            set => SetProperty(ref _vehicleMaintenanceChecksPerformed, value);
        }

        #region Command
        public DelegateCommand PerformNewCheckCommand { get; private set; }
        #endregion

        #endregion

        #region Methods
        public MainViewModel(IDataService dataService, IRegionManager regionManager, INotificationsService notificationsService , IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;

            PerformNewCheckCommand = new DelegateCommand(PerformNewCheckExecute);

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        /// <summary>
        /// This function get any maintenance checks performed on the selected vehicle.
        /// </summary>
        private void GetSelectedVehicleMaintenanceChecks()
        {
            if (SelectedVehicle == null)
            {
                VehicleMaintenanceChecksPerformed = null;
            }
            else
            {
                var checks = _dataService.GetMaintenanceChecksForVehicleByVehicleId(SelectedVehicle.Vehicle.Id);

                if (checks == null || checks.Result == null)
                    return;

                VehicleMaintenanceChecksPerformed = new ObservableCollection<MaintenanceCheck>(checks.Result);               
            }
        }

        #region Event Handlers
        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
            //get any maintenance checks performed on this vehicle.
            GetSelectedVehicleMaintenanceChecks();
        }
        #endregion

        #region Command Handlers
        /// <summary>
        /// This will navigate to the view which allows the user to peform a new check on their vehicle.
        /// </summary>
        private void PerformNewCheckExecute()
        {
            var selectType = new SelectMaintenanceType();
            var viewModel = selectType.DataContext as SelectMaintenanceTypeViewModel;
            selectType.ShowDialog();

            if (viewModel.DialogResult)
            {
                if (viewModel.SelectedMaintenanceCheckType != null)
                {
                    var parameters = new NavigationParameters
                    {
                        { "SelectedTypeId", viewModel.SelectedMaintenanceCheckType.Id },
                        { "SelectedVehicleId", SelectedVehicle.Vehicle.Id}
                    };

                    _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(PerformNewCheck).FullName + parameters);
                }
                else
                {
                    _notificationsService.Alert(START_MAINTENANCE_CHECK_ERROR, NOTIFICATION_HEADER);
                }
            }
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 