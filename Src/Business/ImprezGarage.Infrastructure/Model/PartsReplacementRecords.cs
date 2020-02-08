using System;

namespace ImprezGarage.Infrastructure.Model
{
    public class PartsReplacementRecords
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string PartReplaced { get; set; }
        public string ReplacedWith { get; set; }
        public decimal Price { get; set; }
    }
}
