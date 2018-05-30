//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using System;

    public class MotorbikeCreationViewModel : VehicleCreationViewModel
    {
        private string _registration;
        public string Registration
        {
            get => _registration;
            set
            {
                if (value == _registration)
                    return;

                _registration = value;
                RaisePropertyChanged("Registration");
            }
        }

        private bool _hasValidTax;
        public bool HasValidTax
        {
            get => _hasValidTax;
            set
            {
                SetProperty(ref _hasValidTax, value);
            }
        }

        private DateTime _taxExpiryDate;
        public DateTime TaxExpiryDate
        {
            get => _taxExpiryDate;
            set
            {
                if (value == _taxExpiryDate)
                    return;

                _taxExpiryDate = value;
                RaisePropertyChanged("TaxExpiryDate");
            }
        }

        private bool _hasInsurance;
        public bool HasInsurance
        {
            get => _hasInsurance;
            set
            {
                SetProperty(ref _hasInsurance, value);
                RaisePropertyChanged("HasInsurance");
            }
        }

        private DateTime _insuranceRenewalDate;
        public DateTime InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set
            {
                if (value == _insuranceRenewalDate)
                    return;

                _insuranceRenewalDate = value;
                RaisePropertyChanged("InsuranceRenewalDate");
            }
        }

        public MotorbikeCreationViewModel()
        {
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

        internal override void CleanUp()
        {
            Registration = null;
            TaxExpiryDate = new DateTime();
            InsuranceRenewalDate = new DateTime();
            base.CleanUp();
        }
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 