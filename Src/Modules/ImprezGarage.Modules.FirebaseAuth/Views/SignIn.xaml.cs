using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImprezGarage.Modules.FirebaseAuth.Views
{
    /// <summary>
    /// Interaction logic for SignIn
    /// </summary>
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void Email_OnKeyUp(object sender, KeyEventArgs e)
        {
            EmailLabel.Visibility = string.IsNullOrEmpty(((TextBox)sender).Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Password_OnKeyUp(object sender, KeyEventArgs e)
        {
            PasswordLabel.Visibility = ((PasswordBox)sender).SecurePassword.Length == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((dynamic)DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
