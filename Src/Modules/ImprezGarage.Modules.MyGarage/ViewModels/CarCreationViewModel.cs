//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using System;

    public class CarCreationViewModel : VehicleCreationViewModel
    {
        #region Attributes
        private string _registration;
        private bool _hasValidTax;
        private DateTime _taxExpiryDate;
        private bool _hasInsurance;
        private DateTime _insuranceRenewalDate;
        #endregion

        #region Parameters
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        public bool HasValidTax
        {
            get => _hasValidTax;
            set => SetProperty(ref _hasValidTax, value);
        }

        public DateTime TaxExpiryDate
        {
            get => _taxExpiryDate;
            set => SetProperty(ref _taxExpiryDate, value);
        }

        public bool HasInsurance
        {
            get => _hasInsurance;
            set => SetProperty(ref _hasInsurance, value);
        }

        public DateTime InsuranceRenewalDate
        {
            get => _insuranceRenewalDate;
            set => SetProperty(ref _insuranceRenewalDate, value);
        }
        #endregion

        public CarCreationViewModel()
        {
        }
        
        //public override string this[string columnName]
        //{
        //    get
        //    {
        //        string error = null;

        //        switch (columnName)
        //        {
        //            case "Registration":
        //                if (_registration == null)
        //                {
        //                    error = "Please indicate the registration of this vehicle.";
        //                }
        //                break;
        //            case "TaxExpiryDate":
        //                if (HasValidTax && TaxExpiryDate.Date <= DateTime.Today)
        //                {
        //                    error = "The tax expiry date cannot be earlier than today's date.";
        //                }
        //                break;
        //            case "InsuranceRenewalDate":
        //                if (HasInsurance && InsuranceRenewalDate.Date <= DateTime.Today)
        //                {
        //                    error = "The insurance renewal date cannot be earlier than today's date.";
        //                }
        //                break;
        //        }
                
        //        return (error);
        //    }
        //}
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 