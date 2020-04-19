//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public sealed class VehicleViewModel : BindableBase
    {
        #region Attributes
        private const int DaysAllowanceBeforeReminder = 30;
        private readonly List<string> _reminders;
        
        /// <summary>
        /// Store the injected data service.
        /// </summary>
        private readonly IDataService _dataService;

        /// <summary>
        /// Store the injected notification service.
        /// </summary>
        private readonly INotificationsService _notificationsService;
        #endregion

        #region Properties

        /// <summary>
        /// Store the vehicle itself.
        /// </summary>
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        /// <summary>
        /// Store the type of vehicle.
        /// </summary>
        private VehicleType _vehicleType;
        public VehicleType VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        /// <summary>
        /// Store the date the vehicle was created.
        /// </summary>
        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        /// <summary>
        /// Store the date in which the vehicle was last modified.
        /// </summary>
        private DateTime _dateModified;
        public DateTime DateModified
        {
            get => _dateModified;
            set => SetProperty(ref _dateModified, value);
        }

        /// <summary>
        /// Store the make of the vehicle.
        /// </summary>
        private string _make;
        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        /// <summary>
        /// Store the model of the vehicle.
        /// </summary>
        private string _model;
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        /// <summary>
        /// Store the registration of the vehicle.
        /// </summary>
        private string _registration;
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        /// <summary>
        /// Store the tax renewal date of the vehicle.
        /// </summary>
        private DateTime? _taxExpiryDate;
        public DateTime? TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        /// <summary>
        /// Store the insurance renewal date of the vehicle.
        /// </summary>
        private DateTime? _insuranceRenewalDate;
        public DateTime? InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
        }

        private bool _isReadonly;
        public bool IsReadonly
        {
            get => _isReadonly;
            set => SetProperty(ref _isReadonly, value);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Construct this vehicle view model, store the injected interfaces and instantiate the commands.
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="notificationsService"></param>
        public VehicleViewModel(IDataService dataService, INotificationsService notificationsService)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _reminders = new List<string>();
        }

        /// <summary>
        /// Loads the data from the vehicle passed through are the one already assigned to this viewmodel.
        /// </summary>
        public async void LoadInstance(Vehicle vehicle = null)
        {
            if (vehicle != null)
            {
                Vehicle = vehicle;
            }

            DateCreated = Vehicle.DateCreated;
            DateModified = Vehicle.DateModified;
            Make = Vehicle.Make;
            Model = Vehicle.Model;
            IsReadonly = Vehicle.IsReadonly;

            var type = await _dataService.GetVehicleTypeAsync(Vehicle.VehicleType);

            if (type == null)
                return;

            VehicleType = type;

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


            CheckForVehicleReminders();
        }

        /// <summary>
        /// Go through each vehicle and check for tax expiry dates, insurance renewal dates, MOT dates, service dates. Last time performed a maintenance check etc.
        /// </summary>
        private void CheckForVehicleReminders()
        {
            switch (VehicleType.Name)
            {
                case "Car":
                case "Motorbike":
                    CheckLastMaintenanceCheck();
                    CheckInsuranceRenewalDate();
                    CheckTaxDate();
                    // CheckLastServiceDate();
                    break;
                case "Bicycle":
                    // CheckLastMaintenanceCheck();
                    // GetLastServiceDate();
                    break;
            }

            // Go through all of the reminders and create toast notifications to inform the user.
            foreach (var reminder in _reminders)
            {
                Application.Current.Dispatcher?.Invoke(() => { _notificationsService.Toast(reminder); });
            }
        }

        /// <summary>
        /// Check the tax expiry date of the passed through vehicle and if it runs out in less than 30 days, inform the user.
        /// </summary>
        private void CheckTaxDate()
        {
            if (Vehicle.HasValidTax == false)
                return;

            var taxDate = Convert.ToDateTime(Vehicle.TaxExpiryDate);
            if ((taxDate - DateTime.Now).TotalDays < DaysAllowanceBeforeReminder)
            {
                _reminders.Add($"The tax of vehicle: {Vehicle.Registration} runs out on the : {taxDate.ToShortDateString()}");
            }
        }

        /// <summary>
        /// Check the insurance renewal date of the passed through vehicle and if it runs out in less than 30 days, inform the user.
        /// </summary>
        private void CheckInsuranceRenewalDate()
        {
            if (Vehicle.HasInsurance == false)
                return;

            var insuranceRenewalDate = Convert.ToDateTime(Vehicle.InsuranceRenewalDate);
            if ((insuranceRenewalDate - DateTime.Now).TotalDays < DaysAllowanceBeforeReminder)
            {
                _reminders.Add($"The insurance of vehicle:  {Vehicle.Registration} runs out on the : {insuranceRenewalDate.ToShortDateString()}");
            }
        }

        /// <summary>
        /// Find the last maintenance check performed on the passed through vehicle and if it is more than 30 days old, then add this to the reminders;
        /// </summary>
        private void CheckLastMaintenanceCheck()
        {
            var lastDate = _dataService.LastMaintenanceCheckDateForVehicle(Vehicle.Id);

            if (lastDate == null)
            {
                _reminders.Add($"No maintenance checks have been performed on {Vehicle.Registration}");
                return;
            }

            var date = Convert.ToDateTime(lastDate);
            if ((date - DateTime.Now).TotalDays < DaysAllowanceBeforeReminder)
            {
                _reminders.Add($"A maintenance check was last performed on vehicle {Vehicle.Registration} on: {date.ToShortDateString()}");
            }
        }
        #endregion
    }
}   //ImprezGarage.Infrastructure.ViewModels namespace 