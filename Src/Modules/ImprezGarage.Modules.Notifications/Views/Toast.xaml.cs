//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.Views
{
    using ImprezGarage.Modules.Notifications.ViewModels;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Toast.xaml
    /// </summary>
    public partial class Toast : Window
    {
        public Toast()
        {
            InitializeComponent();
            var viewModel = DataContext as ToastViewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
        
        private void DoubleAnimationUsingKeyFrames_Completed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
        }
    }
}   //ImprezGarage.Modules.Notifications.Views namespace 