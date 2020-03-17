//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
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

        #endregion

        #region Properties
        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        private ObservableCollection<MaintenanceCheckViewModel> _maintenanceChecks;
        public ObservableCollection<MaintenanceCheckViewModel> MaintenanceChecks
        {
            get => _maintenanceChecks;
            set => SetProperty(ref _maintenanceChecks, value);
        }

        #region Command
        public DelegateCommand PerformNewCheckCommand { get; }
        #endregion

        #endregion

        #region Methods
        public MainViewModel(IDataService dataService, IRegionManager regionManager, INotificationsService notificationsService, 
            IUnityContainer container, IVehicleService vehicleService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _container = container;
            _vehicleService = vehicleService;

            PerformNewCheckCommand = new DelegateCommand(PerformNewCheckExecute);

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
        }

        private void GetSelectedVehicleMaintenanceChecks()
        {
            MaintenanceChecks = new ObservableCollection<MaintenanceCheckViewModel>();

            if (SelectedVehicle == null)
                return;

            var performedCheck = _dataService.GetVehicleMaintenanceChecks(SelectedVehicle.Id);

            if (performedCheck == null)
                return;

            foreach (var check in performedCheck.OrderByDescending(o => o.DatePerformed))
            {
                var checkVm = _container.Resolve<MaintenanceCheckViewModel>();
                checkVm.LoadInstance(check, SelectedVehicle);
                MaintenanceChecks.Add(checkVm);
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

            SelectedVehicle = vehicle;
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
            if (SelectedVehicle == null)
            {
                SelectedVehicle = _vehicleService.GetSelectedVehicle();
            }

            GetSelectedVehicleMaintenanceChecks();
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