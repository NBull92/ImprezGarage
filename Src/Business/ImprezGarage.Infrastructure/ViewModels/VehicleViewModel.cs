//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.ViewModels
{
    using Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public sealed class VehicleViewModel : BindableBase
    {
        #region Attributes
        /// <summary>
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
        /// Store the vehicle itself.
        /// </summary>
        private Vehicle _vehicle;

        /// <summary>
        /// Store the type of vehicle.
        /// </summary>
        private VehicleType _vehicleType;

        /// <summary>
        /// Store the date the vehicle was created.
        /// </summary>
        private DateTime _dateCreated;

        /// <summary>
        /// Store the date in which the vehicle was last modified.
        /// </summary>
        private DateTime _dateModified;

        /// <summary>
        /// Store the make of the vehicle.
        /// </summary>
        private string _make;

        /// <summary>
        /// Store the model of the vehicle.
        /// </summary>
        private string _model;

        /// <summary>
        /// Store the registration of the vehicle.
        /// </summary>
        private string _registration;

        /// <summary>
        /// Store the tax renewal date of the vehicle.
        /// </summary>
        private DateTime? _taxExpiryDate;

        /// <summary>
        /// Store the insurance renewal date of the vehicle.
        /// </summary>
        private DateTime? _insuranceRenewalDate;

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
        #endregion

        #region Properties

        /// <summary>
        /// Store the vehicle itself.
        /// </summary>
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        /// <summary>
        /// Store the type of vehicle.
        /// </summary>
        public VehicleType VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        /// <summary>
        /// Store the date the vehicle was created.
        /// </summary>
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        /// <summary>
        /// Store the date in which the vehicle was last modified.
        /// </summary>
        public DateTime DateModified
        {
            get => _dateModified;
            set => SetProperty(ref _dateModified, value);
        }

        /// <summary>
        /// Store the make of the vehicle.
        /// </summary>
        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        /// <summary>
        /// Store the model of the vehicle.
        /// </summary>
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        /// <summary>
        /// Store the registration of the vehicle.
        /// </summary>
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        /// <summary>
        /// Store the tax renewal date of the vehicle.
        /// </summary>
        public DateTime? TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        /// <summary>
        /// Store the insurance renewal date of the vehicle.
        /// </summary>
        public DateTime? InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
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
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Construct this vehicle view model, store the injected interfaces and instantiate the commands.
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="notificationsService"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="loggerService"></param>
        public VehicleViewModel(IDataService dataService, INotificationsService notificationsService, IEventAggregator eventAggregator, ILoggerService loggerService)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;
            _loggerService = loggerService;

            EditVehicleCommand = new DelegateCommand(EditVehicleExecute);
            DeleteVehicleCommand = new DelegateCommand(DeleteVehicleExecute);
        }

        #region Command Handlers
        /// <summary>
        /// Deletes this vehicle in the database.
        /// </summary>
        private void DeleteVehicleExecute()
        {
            if (!_notificationsService.Confirm(ConfirmVehicleDelete))
                return;

            _dataService.DeleteVehicle((error) =>
            {
                if (error != null)
                {
                    _loggerService.LogException(error);
                    return;
                }

                _notificationsService.Alert(VehicleDeleted, NotificationHeader);

                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(null);
            }, Vehicle);
        }

        /// <summary>
        /// Take the current selected vehicle and open a window to edit it.
        /// </summary>
        public void EditVehicleExecute()
        {
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Publish(this);
        }
        #endregion
        
        /// <summary>
        /// Loads the data from the vehicle passed through are the one already assigned to this viewmodel.
        /// </summary>
        public void LoadInstance(Vehicle vehicle = null)
        {
            if (vehicle != null)
            {
                Vehicle = vehicle;
            }

            DateCreated = Vehicle.DateCreated;
            DateModified = Vehicle.DateModified;
            Make = Vehicle.Make;
            Model = Vehicle.Model;

            var type = _dataService.GetVehicleType(Vehicle.VehicleType);

            if (type?.Result == null)
                return;

            VehicleType = type.Result;

            switch (VehicleType.Id)
            {
                case 1:
                    Registration = Vehicle.Registration;
                    TaxExpiryDate = Vehicle.TaxExpiryDate;
                    InsuranceRenewalDate = Vehicle.InsuranceRenewalDate;
                    break;
                case 2:
                    Registration = Vehicle.Registration;
                    TaxExpiryDate = Vehicle.TaxExpiryDate;
                    InsuranceRenewalDate = Vehicle.InsuranceRenewalDate;
                    break;
                case 3:
                    break;
            }
        }
        #endregion
    }
}   //ImprezGarage.Infrastructure.ViewModels namespace 