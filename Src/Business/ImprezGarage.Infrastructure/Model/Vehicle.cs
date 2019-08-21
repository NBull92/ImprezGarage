
namespace ImprezGarage.Infrastructure.Model
{
    using System;

    public class Vehicle
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int VehicleType { get; set; }
        public string Registration { get; set; }
        public bool HasInsurance { get; set; }
        public bool HasValidTax { get; set; }
        public bool IsManual { get; set; }
        public bool HasMot { get; set; }
        public DateTime? TaxExpiryDate { get; set; }
        public DateTime? InsuranceRenewalDate { get; set; }
        public DateTime? MotExpiryDate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int CurrentMileage { get; set; }
        public int MileageOnPurchase { get; set; }
        public string UserId { get; set; }
    }
}
