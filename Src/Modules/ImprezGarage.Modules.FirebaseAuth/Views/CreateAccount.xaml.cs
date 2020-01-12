using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImprezGarage.Modules.FirebaseAuth.Views
{
    /// <summary>
    /// Interaction logic for CreateAccount
    /// </summary>
    public partial class CreateAccount : UserControl
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void Email_OnKeyUp(object sender, KeyEventArgs e)
        {
            EmailLabel.Visibility = string.IsNullOrEmpty(((TextBox) sender).Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Password_OnKeyUp(object sender, KeyEventArgs e)
        {
            PasswordLabel.Visibility = string.IsNullOrEmpty(((TextBox)sender).Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RePassword_OnKeyUp(object sender, KeyEventArgs e)
        {
            RePasswordLabel.Visibility = string.IsNullOrEmpty(((TextBox)sender).Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
