﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

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

    public class MainViewModel : BindableBase, INavigationAware
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly INotificationsService _notificationsService;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

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
        public MainViewModel(IDataService dataService, IRegionManager regionManager, INotificationsService notificationsService , IEventAggregator eventAggregator,
            IUnityContainer container)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _container = container;

            PerformNewCheckCommand = new DelegateCommand(PerformNewCheckExecute);

            eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        private void GetSelectedVehicleMaintainanceChecks()
        {
            if (SelectedVehicle == null)
            {
                MaintainanceChecks = null;
            }
            else
            {
                var performedCheck = _dataService.GetMaintenanceChecksForVehicleByVehicleId(SelectedVehicle.Vehicle.Id);

                if (performedCheck == null || performedCheck.Result == null)
                {
                    MaintainanceChecks = null;
                    return;
                }

                MaintainanceChecks = new ObservableCollection<MaintenanceCheckViewModel>();

                foreach (var check in performedCheck.Result.OrderByDescending(o => o.DatePerformed))
                {
                    var checkVm = _container.Resolve<MaintenanceCheckViewModel>();
                    checkVm.LoadInstance(check, SelectedVehicle);
                    MaintainanceChecks.Add(checkVm);
                }
            }
        }

        #region Event Handlers
        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
            GetSelectedVehicleMaintainanceChecks();
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
            if (refresh != null && Convert.ToBoolean(refresh) == true)
            {
                GetSelectedVehicleMaintainanceChecks();
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
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 