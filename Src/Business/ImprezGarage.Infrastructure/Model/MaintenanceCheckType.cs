namespace ImprezGarage.Infrastructure.Model
{
    public class MaintenanceCheckType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? OccurenceMonthly { get; set; }
        public int? OccurenceMiles { get; set; }
    }
}
