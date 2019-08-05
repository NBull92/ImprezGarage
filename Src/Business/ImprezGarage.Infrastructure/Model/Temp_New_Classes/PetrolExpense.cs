
namespace ImprezGarage.Infrastructure.Model.Temp_New_Classes
{
    using System;

    public class PetrolExpense
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public DateTime? DateEntered { get; set; }
        public int? VehicleId { get; set; }
    }
}
