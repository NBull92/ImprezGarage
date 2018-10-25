//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using Infrastructure;
    using Infrastructure.Services;
    using ImprezGarage.Infrastructure.ViewModels;
    using Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Windows.Input;
    using ImprezGarage.Infrastructure.Model;

    public class MaintenanceCheckViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly INotificationsService _notificationsService;
        private readonly IEventAggregator _eventAggregator;

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
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;

            OpenMaintenanceCheck = new DelegateCommand<MouseButtonEventArgs>(OpenMaintenanceCheckExecute);
            EditMaintenanceCheckCommand = new DelegateCommand(EditMaintenanceCheckExecute);
            DeleteMaintenanceCheckCommand = new DelegateCommand(DeleteMaintenanceCheckExecute);
        }


        #region Command Handlers
        private void DeleteMaintenanceCheckExecute()
        {
            if (!_notificationsService.Confirm(DeleteMaintenanceCheck))
                return;

            _dataService.DeleteMaintenanceCheck((error) => 
            {
                if(error != null)
                {

                }

                _notificationsService.Alert(DeletedSuccessfully);
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(SelectedVehicle);
            }, Id);
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
            
            if (type == null || type.Result == null)
                return;

            MaintenanceCheckType = type.Result;
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 