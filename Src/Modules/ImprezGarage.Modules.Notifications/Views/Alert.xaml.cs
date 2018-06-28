//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.Views
{
    using ImprezGarage.Modules.Notifications.ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class Alert : MetroWindow
    {
        public Alert()
        {
            InitializeComponent();
            var viewModel = DataContext as AlertViewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.Notifications.Views namespace 