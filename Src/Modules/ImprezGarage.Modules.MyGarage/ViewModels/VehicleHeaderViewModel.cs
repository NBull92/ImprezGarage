
using System;
using ImprezGarage.Infrastructure.Model;
using ImprezGarage.Infrastructure.Services;

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.ViewModels;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;

    public class VehicleHeaderViewModel : BindableBase
    {    /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Store the injected data service.
        /// </summary>
        private readonly IDataService _dataService;

        /// <summary>
        /// Store the injected notification service.
        /// </summary>
        private readonly INotificationsService _notificationsService;

        /// <summary>
        /// Store the injected logger service.
        /// </summary>
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Store the constant header for a notification alert.
        /// </summary>
        private const string NotificationHeader = "Alert!";

        /// <summary>
        /// Store the constant for when a vehicle is deleted.
        /// </summary>
        private const string VehicleDeleted = "Vehicle deleted sucessfully!";

        /// <summary>
        /// Store the constant for when a vehicle is about to be deleted.
        /// </summary>
        private const string ConfirmVehicleDelete = "Are you sure you wish to delete this vehicle?";

        private VehicleViewModel _selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        #region Commands
        /// <summary>
        /// Command for editing this vehicle.
        /// </summary>
        public DelegateCommand EditVehicleCommand { get; set; }

        /// <summary>
        /// Command for deleting this vehicle.
        /// </summary>
        public DelegateCommand DeleteVehicleCommand { get; set; }
        public DelegateCommand Repairs { get; }
        #endregion

        public VehicleHeaderViewModel(IDataService dataService, INotificationsService notificationsService,
            IEventAggregator eventAggregator, ILoggerService loggerService)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;
            _loggerService = loggerService;

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);

            EditVehicleCommand = new DelegateCommand(EditVehicleExecute);
            DeleteVehicleCommand = new DelegateCommand(DeleteVehicleExecute);
            Repairs = new DelegateCommand(OnRepairs);
        }

        private void OnSelectedVehicleChanged(VehicleViewModel model)
        {
            SelectedVehicle = model;
        }

        #region Command Handlers
        /// <summary>
        /// Deletes this vehicle in the database.
        /// </summary>
        private void DeleteVehicleExecute()
        {
            if (!_notificationsService.Confirm(ConfirmVehicleDelete))
                return;

            try
            {
                _dataService.DeleteVehicle(SelectedVehicle.Vehicle);
                _notificationsService.Alert(VehicleDeleted, NotificationHeader);
                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(null);
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }

        /// <summary>
        /// Take the current selected vehicle and open a window to edit it.
        /// </summary>
        public void EditVehicleExecute()
        {
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Publish(SelectedVehicle);
        }

        private void OnRepairs()
        {
            var vm = new ReportRepairViewModel(_dataService, _notificationsService, _loggerService)
            {
                VehicleId = SelectedVehicle.Vehicle.Id
            };
            var repair = new ReportRepair(vm);
            repair.ShowDialog();
        }
        #endregion

    }
}
