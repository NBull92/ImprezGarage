namespace ImprezGarage.Modules.PerformChecks
{
    using Infrastructure.Model.Temp_New_Classes;

    public class Option
    {
        public int Id { get; set; }
        public int MaintenanceCheck { get; set; }
        public MaintenanceCheckOption MaintenanceOption { get; set; }
        public bool? IsChecked { get; set; }
        public string Notes { get; set; }

        public Option(MaintenanceCheckOption option, PerformedMaintenanceOption maintenanceOption = null, bool isChecked = false)
        {
            MaintenanceOption = option;
            IsChecked = isChecked;

            if (maintenanceOption == null)
                return;

            Id = maintenanceOption.Id;
            MaintenanceCheck = maintenanceOption.MaintenanceCheck;
            IsChecked = maintenanceOption.IsChecked;
            Notes = maintenanceOption.Notes;
        }
    }
}
