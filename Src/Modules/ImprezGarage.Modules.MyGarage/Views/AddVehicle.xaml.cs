//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.Views
{
    using ImprezGarage.Modules.MyGarage.ViewModels;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AddVehicle
    /// </summary>
    public partial class AddVehicle : UserControl
    {
        public AddVehicle()
        {
            InitializeComponent();
            var viewModel = DataContext as AddVehicleViewModel;
            viewModel.ClosingRequest += (sender, e) => ((Window)Parent).Close();
        }
    }
}   //ImprezGarage.Modules.MyGarage.Views namespace 