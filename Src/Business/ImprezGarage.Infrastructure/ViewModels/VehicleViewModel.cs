﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.ViewModels
{
    using ImprezGarage.Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public class VehicleViewModel : BindableBase
    {
        #region Attributes
        private Vehicle _vehicle;
        private VehicleType _vehicleType;
        private DateTime _dateCreated;
        private DateTime _dateModified;
        private string _make;
        private string _model;
        private string _registration;
        private DateTime? _taxExpiryDate;
        private DateTime? _insuranceRenewalDate;
        private const string NOTIFICATION_HEADER = "Alert!";
        private const string VEHICLE_DELETED = "Vehicle deleted sucessfully!";
        private const string CONFIRM_VEHICLE_DELETE = "Are you sure you wish to delete this vehicle?";
        private readonly IEventAggregator _eventAggregator;
        public IDataService _dataService { get; private set; }
        public INotificationsService _notificationsService { get; private set; }
        #endregion

        #region Properties

        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        public VehicleType VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        public DateTime DateModified
        {
            get => _dateModified;
            set => SetProperty(ref _dateModified, value);
        }

        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        public DateTime? TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        public DateTime? InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
        }

        #region Commands
        public virtual DelegateCommand EditVehicleCommand { get; set; }
        public virtual DelegateCommand DeleteVehicleCommand { get; set; }
        #endregion
        #endregion

        #region Methods
        public VehicleViewModel(IDataService dataService, INotificationsService notificationsService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;

            EditVehicleCommand = new DelegateCommand(EditVehicleExecute);
            DeleteVehicleCommand = new DelegateCommand(DeleteVehicleExecute);
        }

        #region Command Handlers
        /// <summary>
        /// Deletes this vehicle in the database.
        /// </summary>
        private void DeleteVehicleExecute()
        {
            if (!_notificationsService.Confirm(CONFIRM_VEHICLE_DELETE))
                return;

            _dataService.DeleteVehicle((error) =>
            {
                if (error != null)
                {
                    return;
                }

                _notificationsService.Alert(VEHICLE_DELETED, NOTIFICATION_HEADER);

                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(null);
            }, Vehicle);
        }

        /// <summary>
        /// Take the current selected vechiel and open a window to edit it.
        /// </summary>
        public void EditVehicleExecute()
        {
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Publish(this);
        }
        #endregion

        public void LoadInstanceViaId(int selectedVehicleId)
        {
            var vehicle = _dataService.GetVehicleByItsId(selectedVehicleId);

            if (vehicle == null || vehicle.Result == null)
                return;

            LoadInstance(vehicle.Result);
        }

        /// <summary>
        /// Calls load instance to happen again but refresh the data from the vehicle in this view model.
        /// </summary>
        public void Refresh()
        {
            LoadInstance();
        }

        /// <summary>
        /// Loads the data from the vehicle passed through are the one already assigned to this viewmodel.
        /// </summary>
        /// <param name="vehicle"></param>
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

            if (type == null || type.Result == null)
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