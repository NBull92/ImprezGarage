//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vehicle
    {
        public int Id { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public int VehicleType { get; set; }
        public string Registration { get; set; }
        public Nullable<System.DateTime> TaxExpiryDate { get; set; }
        public Nullable<System.DateTime> InsuranceRenewalDate { get; set; }
        public string FriendlyName { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public Nullable<bool> HasInsurance { get; set; }
        public Nullable<bool> HasValidTax { get; set; }
        public Nullable<bool> IsManual { get; set; }
        public Nullable<int> CurrentMilage { get; set; }
        public Nullable<int> MilageOnPurchase { get; set; }
    
        public virtual VehicleType VehicleType1 { get; set; }
    }
}
