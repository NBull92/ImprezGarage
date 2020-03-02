//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.Views
{
    using ViewModels;
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
            viewModel.CloseRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.Notifications.Viewsnamespace 