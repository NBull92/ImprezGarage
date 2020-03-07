//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels.CreationViewModels
{
    using Infrastructure.Model;
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
        /// The Mileage the car had when the user bought the car.
        /// </summary>
        public int MileageOnPurchase
        {
            get => _mileageOnPurchase;
            set => SetProperty(ref _mileageOnPurchase, value);
        }

        /// <summary>
        /// Current Mileage of the car.
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
        public override void Setup(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
            InsuranceRenewalDate = DateTime.Now;
            TaxExpiryDate = DateTime.Now;
            base.Setup(vehicleType);
        }

        public override void SaveNew(Vehicle vehicle)
        {
            vehicle.Registration = Registration;
            vehicle.HasInsurance = HasInsurance;
            vehicle.HasValidTax = HasValidTax;
            vehicle.HasMot = HasMot;
            vehicle.CurrentMileage = CurrentMileage;
            vehicle.MileageOnPurchase = MileageOnPurchase;
            vehicle.IsManual = IsManual;

            if (vehicle.HasInsurance)
            {
                vehicle.InsuranceRenewalDate = InsuranceRenewalDate;
            }


            if (vehicle.HasValidTax)
            {
                vehicle.TaxExpiryDate = TaxExpiryDate;
            }

            base.SaveNew(vehicle);
        }

        public override void Update(Vehicle vehicle)
        {
            vehicle.Registration = Registration;
            vehicle.Make = Make;
            vehicle.Model = Model;
            vehicle.HasInsurance = HasInsurance;
            vehicle.HasValidTax = HasValidTax;
            vehicle.HasMot = HasMot;
            vehicle.CurrentMileage = CurrentMileage;
            vehicle.MileageOnPurchase = MileageOnPurchase;
            vehicle.IsManual = IsManual;

            if (HasValidTax)
            {
                vehicle.InsuranceRenewalDate = InsuranceRenewalDate;
            }

            if (HasValidTax)
            {
                vehicle.TaxExpiryDate = TaxExpiryDate;
            }

            if (vehicle.HasMot)
            {
                vehicle.MotExpiryDate = MotExpiryDate;
            }

            base.Update(vehicle);
        }

        public override bool CanSave()
        {
            return !string.IsNullOrEmpty(Registration);
        }

        public override void EditInitialise(Vehicle vehicle)
        {
            ShowEditView = true;
            Registration = vehicle.Registration;
            Make = vehicle.Make;
            Model = vehicle.Model;
            HasInsurance = Convert.ToBoolean(vehicle.HasInsurance);
            HasValidTax = Convert.ToBoolean(vehicle.HasValidTax);
            HasMot = Convert.ToBoolean(vehicle.HasMot);
            InsuranceRenewalDate = vehicle.HasInsurance ? Convert.ToDateTime(vehicle.InsuranceRenewalDate) : DateTime.Now;
            TaxExpiryDate = vehicle.HasValidTax ? Convert.ToDateTime(vehicle.TaxExpiryDate) : DateTime.Now;
            MotExpiryDate = vehicle.HasMot ? Convert.ToDateTime(vehicle.MotExpiryDate) : DateTime.Now;
            CurrentMileage = Convert.ToInt32(vehicle.CurrentMileage);
            MileageOnPurchase = Convert.ToInt32(vehicle.MileageOnPurchase);
            IsManual = Convert.ToBoolean(vehicle.IsManual);
        }

        /// <summary>
        /// Reset all of the properties to empty and then call the parent's dispose function.
        /// </summary>
        public override void CleanUp()
        {
            Registration = null;
            Make = null;
            Model = null;
            HasInsurance = false;
            HasValidTax = false;
            HasMot = false;
            InsuranceRenewalDate = new DateTime();
            TaxExpiryDate = new DateTime();
            MotExpiryDate = new DateTime();
            CurrentMileage = 0;
            MileageOnPurchase = 0;
            IsManual = false;

            Dispose();
        }
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 