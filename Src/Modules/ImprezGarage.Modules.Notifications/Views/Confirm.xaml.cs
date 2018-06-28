//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.Views
{
    using ImprezGarage.Modules.Notifications.ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : MetroWindow
    {
        public Confirm()
        {
            InitializeComponent();
            var viewModel = DataContext as ConfirmViewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.Notifications.Viewsnamespace 