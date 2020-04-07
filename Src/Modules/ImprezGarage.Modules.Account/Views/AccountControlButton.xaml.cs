
namespace ImprezGarage.Modules.Account.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public partial class AccountControlButton : UserControl
    {
        public AccountControlButton()
        {
            InitializeComponent();
        }

        private void BtnAccountOnClick(object sender, RoutedEventArgs e)
        {
            var cm = ((Button)sender).ContextMenu;
            if (cm == null)
                return;

            cm.PlacementTarget = (Button)sender;
            cm.Placement = PlacementMode.Bottom;
            cm.IsOpen = true;
        }

        private void BtnAccountOnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}
