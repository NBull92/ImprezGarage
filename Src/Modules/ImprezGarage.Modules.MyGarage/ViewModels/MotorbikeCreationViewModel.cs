//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.Services;
    using System;

    public class MotorbikeCreationViewModel : VehicleCreationViewModel
    {
        #region Attributes
        /// <summary>
        /// Motorbike registration.
        /// </summary>
        private string _registration;

        /// <summary>
        /// Does the Motorbike have any tax.
        /// </summary>
        private bool _hasValidTax;

        /// <summary>
        /// The Motorbike's current tax expiry date.
        /// </summary>
        private DateTime _taxExpiryDate;

        /// <summary>
        /// Does the Motorbike have any insurance.
        /// </summary>
        private bool _hasInsurance;

        /// <summary>
        /// The Motorbike's current insurance renewal date.
        /// </summary>
        private DateTime _insuranceRenewalDate;
        #endregion

        #region Parameters
        /// <summary>
        /// Motorbike registration.
        /// </summary>
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        /// <summary>
        /// Does the Motorbike have any tax.
        /// </summary>
        public bool HasValidTax
        {
            get => _hasValidTax;
            set => SetProperty(ref _hasValidTax, value);
        }

        /// <summary>
        /// The Motorbike's current tax expiry date.
        /// </summary>
        public DateTime TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        /// <summary>
        /// Does the Motorbike have any insurance.
        /// </summary>
        public bool HasInsurance
        {
            get => _hasInsurance;
            set => SetProperty(ref _hasInsurance, value);
        }

        /// <summary>
        /// The Motorbike's current insurance renewal date.
        /// </summary>
        public DateTime InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
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

        public override string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "Registration":
                        if (_registration == null)
                        {
                            error = "Please indicate the registration of this vehicle.";
                        }
                        break;
                    case "TaxExpiryDate":
                        if (HasValidTax && TaxExpiryDate.Date <= DateTime.Today)
                        {
                            error = "The tax expiry date cannot be earlier than today's date.";
                        }
                        break;
                    case "InsuranceRenewalDate":
                        if (HasInsurance && InsuranceRenewalDate.Date <= DateTime.Today)
                        {
                            error = "The insurance renewal date cannot be earlier than today's date.";
                        }
                        break;
                }

                return (error);
            }
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