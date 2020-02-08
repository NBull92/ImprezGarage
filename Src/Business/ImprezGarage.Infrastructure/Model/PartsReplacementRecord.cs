
namespace ImprezGarage.Infrastructure.Model
{
    using System;

    public class PartsReplacementRecord
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string PartReplaced { get; set; }
        public string ReplacedWith { get; set; }
        public double Price { get; set; }
        public int VehicleId { get; set; }
    }
}
