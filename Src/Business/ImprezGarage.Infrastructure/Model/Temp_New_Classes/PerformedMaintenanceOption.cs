namespace ImprezGarage.Infrastructure.Model.Temp_New_Classes
{
    public class PerformedMaintenanceOption
    {
        public int Id { get; set; }
        public int MaintenanceCheck { get; set; }
        public int MaintenanceOption { get; set; }
        public bool? IsChecked { get; set; }
        public string Notes { get; set; }
    }
}
