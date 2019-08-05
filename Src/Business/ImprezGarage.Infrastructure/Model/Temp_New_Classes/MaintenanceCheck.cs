
namespace ImprezGarage.Infrastructure.Model.Temp_New_Classes
{
    using System;

    public class MaintenanceCheck
    {
        public int Id { get; set; }
        public DateTime? DatePerformed { get; set; }
        public int? MaintenanceCheckType { get; set; }
        public int? VehicleId { get; set; }
    }
}
