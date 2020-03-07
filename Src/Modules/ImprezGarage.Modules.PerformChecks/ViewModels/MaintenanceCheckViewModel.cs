//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------


namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Windows.Input;
    using Views;

    public class MaintenanceCheckViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILoggerService _loggerService;
        private readonly IVehicleService _vehicleService;

        private const string DeleteMaintenanceCheck = "Are you sure you wish to delete this maintenance check?";
        private const string DeletedSuccessfully = "Maintenance check deleted successfuly!";

        private int _id;
        private VehicleViewModel _selectedVehicle;
        private MaintenanceCheckType _maintenanceCheckType;
        private DateTime _datePerformed;
        #endregion

        #region Properties
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public MaintenanceCheckType MaintenanceCheckType
        {
            get => _maintenanceCheckType;
            set => SetProperty(ref _maintenanceCheckType, value);
        }

        public DateTime DatePerformed
        {
            get => _datePerformed;
            set => SetProperty(ref _datePerformed, value);
        }

        #region Commands
        public DelegateCommand<MouseButtonEventArgs> OpenMaintenanceCheck { get; set; }
        public DelegateCommand EditMaintenanceCheckCommand { get; set; }
        public DelegateCommand DeleteMaintenanceCheckCommand { get; set; }
        #endregion
        #endregion
        
        #region Methods
        public MaintenanceCheckViewModel(IDataService dataService, IRegionManager regionManager, INotificationsService notificationsService,
            ILoggerService loggerService, IVehicleService vehicleService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _loggerService = loggerService;
            _vehicleService = vehicleService;

            OpenMaintenanceCheck = new DelegateCommand<MouseButtonEventArgs>(OpenMaintenanceCheckExecute);
            EditMaintenanceCheckCommand = new DelegateCommand(EditMaintenanceCheckExecute);
            DeleteMaintenanceCheckCommand = new DelegateCommand(DeleteMaintenanceCheckExecute);
        }


        #region Command Handlers
        private void DeleteMaintenanceCheckExecute()
        {
            if (!_notificationsService.Confirm(DeleteMaintenanceCheck))
                return;

            try
            {
                _dataService.DeleteMaintenanceCheck(Id);
                _notificationsService.Alert(DeletedSuccessfully);
                if (SelectedVehicle != null)
                {
                    _vehicleService.RaiseSelectedVehicleChanged(SelectedVehicle.Vehicle);
                }
                else
                {
                    _vehicleService.ClearSelectedVehicle();
                }

            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }

        private void EditMaintenanceCheckExecute()
        {
            EditMaintenceCheck();
        }

        private void OpenMaintenanceCheckExecute(MouseButtonEventArgs obj)
        {
            if (obj.ClickCount == 2)
            {
                EditMaintenceCheck();
            }
        }
        #endregion

        private void EditMaintenceCheck()
        {
            var parameters = new NavigationParameters
            {
                { "MaintenanceCheckId", Id },
                { "SelectedVehicleId", SelectedVehicle.Vehicle.Id},
                { "IsEditMode", true }
            };

            _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(PerformNewCheck).FullName + parameters);
        }

        internal void LoadInstance(MaintenanceCheck check, VehicleViewModel selectedVehicle)
        {
            Id = check.Id;

            SelectedVehicle = selectedVehicle;

            DatePerformed = new DateTime(check.DatePerformed.Value.Year, check.DatePerformed.Value.Month, check.DatePerformed.Value.Day);

            var type = _dataService.GetMaintenanceCheckTypeById(Convert.ToInt32(check.MaintenanceCheckType));
            
            if (type == null)
                return;

            MaintenanceCheckType = type;
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 