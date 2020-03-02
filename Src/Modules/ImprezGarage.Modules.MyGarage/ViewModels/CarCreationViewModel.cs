//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using ImprezGarage.Infrastructure.Model;
    using System;

    public class CarCreationViewModel : VehicleCreationViewModel
    {
        #region Attributes
        /// <summary>
        /// Car registration.
        /// </summary>
        private string _registration;
        
        /// <summary>
        /// Does the car have any tax.
        /// </summary>
        private bool _hasValidTax;

        /// <summary>
        /// The car's current tax expiry date.
        /// </summary>
        private DateTime _taxExpiryDate;

        /// <summary>
        /// Does the car have any insurance.
        /// </summary>
        private bool _hasInsurance;

        /// <summary>
        /// The car's current insurance renewal date.
        /// </summary>
        private DateTime _insuranceRenewalDate;

        /// <summary>
        /// Has the car had an MOT.
        /// </summary>
        private bool _hasMot;

        /// <summary>
        /// The milage the car had when the user bought the car.
        /// </summary>
        private int _mileageOnPurchase;

        /// <summary>
        /// Current milage of the car.
        /// </summary>
        private int _currentMileage;

        /// <summary>
        /// Mot expiry date of the car.
        /// </summary>
        private DateTime _motExpiryDate;

        /// <summary>
        /// Is this car a manual or automatic car.
        /// </summary>
        private bool _isManual;
        #endregion

        #region Parameters
        /// <summary>
        /// Car registration.
        /// </summary>
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        /// <summary>
        /// Does the car have any tax.
        /// </summary>
        public bool HasValidTax
        {
            get => _hasValidTax;
            set => SetProperty(ref _hasValidTax, value);
        }

        /// <summary>
        /// The car's current tax expiry date.
        /// </summary>
        public DateTime TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        /// <summary>
        /// Does the car have any insurance.
        /// </summary>
        public bool HasInsurance
        {
            get => _hasInsurance;
            set => SetProperty(ref _hasInsurance, value);
        }

        /// <summary>
        /// The car's current insurance renewal date.
        /// </summary>
        public DateTime InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
        }
        
        /// <summary>
        /// Is this car a manual or automatic car.
        /// </summary>
        public bool IsManual
        {
            get => _isManual;
            set => SetProperty(ref _isManual, value);
        }

        /// <summary>
        /// Has the car had an MOT.
        /// </summary>
        public bool HasMot
        {
            get => _hasMot;
            set => SetProperty(ref _hasMot, value);
        }

        /// <summary>
        /// Mot expiry date of the car.
        /// </summary>
        public DateTime MotExpiryDate
        {
            get => _motExpiryDate;
            set => SetProperty(ref _motExpiryDate, value);
        }

        /// <summary>
        /// The milage the car had when the user bought the car.
        /// </summary>
        public int MileageOnPurchase
        {
            get => _mileageOnPurchase;
            set => SetProperty(ref _mileageOnPurchase, value);
        }

        /// <summary>
        /// Current milage of the car.
        /// </summary>
        public int CurrentMileage
        {
            get => _currentMileage;
            set => SetProperty(ref _currentMileage, value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Override the setup function of the parent class.
        /// Set the current insurance and tax date.
        /// Store the vehicle type also.
        /// </summary>
        internal override void Setup(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
            InsuranceRenewalDate = DateTime.Now;
            TaxExpiryDate = DateTime.Now;
            base.Setup(vehicleType);
        }

        /// <summary>
        /// Reset all of the properties to empty and then call the parent's dispose fucntion.
        /// </summary>
        public void CleanUp()
        {
            Registration = null;
            TaxExpiryDate = new DateTime();
            InsuranceRenewalDate = new DateTime();
            Dispose();
        }
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 