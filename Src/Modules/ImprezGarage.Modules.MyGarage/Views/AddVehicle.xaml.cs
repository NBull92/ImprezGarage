//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.Views
{
    using ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for AddVehicle
    /// </summary>
    public partial class AddVehicle : MetroWindow
    {
        public AddVehicle()
        {
            InitializeComponent();
            var viewModel = DataContext as AddVehicleViewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.MyGarage.Views namespace 