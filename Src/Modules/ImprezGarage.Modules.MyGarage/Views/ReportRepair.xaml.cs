
namespace ImprezGarage.Infrastructure
{
    using ViewModels;
    using BaseClasses;

    /// <summary>
    /// Interaction logic for ReportRepair.xaml
    /// </summary>
    public partial class ReportRepair : DialogViewBase
    {
        public ReportRepair(ReportRepairViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            model.CloseRequest += (sender, e) => Close();
        }
    }
}
