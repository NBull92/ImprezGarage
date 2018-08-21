
namespace ImprezGarage.Modules.Settings.Views
{
    using ImprezGarage.Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using System.Windows.Controls;
    
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();

            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            DataContext = settingsService.GetConfiguration();
        }
    }
}
