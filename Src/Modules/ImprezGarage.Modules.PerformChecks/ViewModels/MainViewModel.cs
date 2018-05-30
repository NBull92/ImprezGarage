//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using ImprezGarage.Modules.PerformChecks.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Collections.ObjectModel;

    public class MainViewModel : BindableBase, INavigationAware
    {
        private const string START_MAINTENANCE_CHECK_ERROR = "An error occured when attempting to start a maintenance check.";
        private const string NOTIFICATION_HEADER = "Alert!";

        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private VehicleViewModel _selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                RaisePropertyChanged("SelectedVehicle");
            }
        }

        private ObservableCollection<MaintenanceCheck> _vehicleMaintenanceChecksPerformed;
        public ObservableCollection<MaintenanceCheck> VehicleMaintenanceChecksPerformed
        {
            get => _vehicleMaintenanceChecksPerformed;
            set
            {
                _vehicleMaintenanceChecksPerformed = value;
                RaisePropertyChanged("VehicleMaintenanceChecksPerformed");
            }
        }

        public InteractionRequest<INotification> NotificationRequest { get; set; }

        public DelegateCommand PerformNewCheckCommand { get; private set; }

        public MainViewModel(IDataService dataService, IDialogService dialogService, IRegionManager regionManager
            , IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            NotificationRequest = new InteractionRequest<INotification>();

            PerformNewCheckCommand = new DelegateCommand(PerformNewCheckExecute);

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
            //get any maintenance checks performed on this vehicle.
            GetSelectedVehicleMaintenanceChecks();
        }

        #region Command Handlers
        /// <summary>
        /// This will navigate to the view which allows the user to peform a new check on their vehicle.
        /// </summary>
        private void PerformNewCheckExecute()
        {
            _dialogService.ShowWindow((viewModel, error) =>
            {
                if (error != null)
                {
                    return;
                }

                if (viewModel == null)
                    return;

                if (((SelectMaintenanceTypeViewModel)viewModel).DialogResult)
                {
                    if (((SelectMaintenanceTypeViewModel)viewModel).SelectedMaintenanceCheckType != null)
                    {
                        var parameters = new NavigationParameters
                        {
                            { "SelectedTypeId", ((SelectMaintenanceTypeViewModel)viewModel).SelectedMaintenanceCheckType.Id },
                            { "SelectedVehicleId", SelectedVehicle.Vehicle.Id}
                        };

                        _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(PerformNewCheck).FullName + parameters);
                    }
                    else
                    {
                        NotificationRequest.Raise(new Notification { Title = NOTIFICATION_HEADER, Content = START_MAINTENANCE_CHECK_ERROR });
                    }
                }
            }, new SelectMaintenanceType(), 402, 137);
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
                _dataService.GetMaintenanceChecksForVehicleByVehicleId((checks, error) =>
                {
                    if (error != null)
                    {

                    }

                    if (checks == null)
                        return;

                    VehicleMaintenanceChecksPerformed = new ObservableCollection<MaintenanceCheck>(checks);
                }, SelectedVehicle.Vehicle.Id);
            }
        }
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 