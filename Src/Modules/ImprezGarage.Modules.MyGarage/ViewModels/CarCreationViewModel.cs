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
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 