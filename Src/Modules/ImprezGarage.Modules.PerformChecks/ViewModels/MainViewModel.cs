//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using ImprezGarage.Infrastructure.Model;

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Views;

    public class MainViewModel : BindableBase, INavigationAware, IDisposable
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly INotificationsService _notificationsService;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IVehicleService _vehicleService;

        private const string StartMaintenanceCheckError = "An error occured when attempting to start a maintenance check.";
        private const string NotificationHeader = "Alert!";

        private VehicleViewModel _selectedVehicle;
        private ObservableCollection<MaintenanceCheckViewModel> _maintainanceChecks;
        #endregion

        #region Properties
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public ObservableCollection<MaintenanceCheckViewModel> MaintainanceChecks
        {
            get => _maintainanceChecks;
            set => SetProperty(ref _maintainanceChecks, value);
        }

        #region Command
        public DelegateCommand PerformNewCheckCommand { get; }
        #endregion

        #endregion

        #region Methods
        public MainViewModel(IDataService dataService, IRegionManager regionManager, INotificationsService notificationsService, 
            IEventAggregator eventAggregator, IUnityContainer container, IVehicleService vehicleService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _container = container;
            _vehicleService = vehicleService;

            PerformNewCheckCommand = new DelegateCommand(PerformNewCheckExecute);

            //eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
        }

        private void GetSelectedVehicleMaintenanceChecks()
        {
            MaintainanceChecks = new ObservableCollection<MaintenanceCheckViewModel>();

            if (SelectedVehicle == null)
                return;

            var performedCheck = _dataService.GetVehicleMaintenanceChecks(SelectedVehicle.Vehicle.Id);

            if (performedCheck == null)
                return;

            foreach (var check in performedCheck.OrderByDescending(o => o.DatePerformed))
            {
                var checkVm = _container.Resolve<MaintenanceCheckViewModel>();
                checkVm.LoadInstance(check, SelectedVehicle);
                MaintainanceChecks.Add(checkVm);
            }
        }

        #region Event Handlers
        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            if (vehicle == null)
            {
                SelectedVehicle = null;
                return;
            }

            var model = new VehicleViewModel(_dataService, _notificationsService);
            model.LoadInstance(vehicle);
            SelectedVehicle = model;
            GetSelectedVehicleMaintenanceChecks();
        }
        #endregion

        #region Command Handlers
        /// <summary>
        /// This will navigate to the view which allows the user to perform a new check on their vehicle.
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
                    _notificationsService.Alert(StartMaintenanceCheckError, NotificationHeader);
                }
            }
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var refresh = navigationContext.Parameters["Refresh"];

            //check it is not null.
            if (refresh != null && Convert.ToBoolean(refresh))
            {
                GetSelectedVehicleMaintenanceChecks();
            }
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

        public void Dispose()
        {
            _container?.Dispose();
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
        }
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 