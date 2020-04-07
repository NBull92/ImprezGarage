//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.Views
{
    using ViewModels;
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
            viewModel.CloseRequest += (sender, e) => Close();
        }
        
        private void DoubleAnimationUsingKeyFrames_Completed(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}   //ImprezGarage.Modules.Notifications.Views namespace 