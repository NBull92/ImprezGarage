//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.Views
{
    using ImprezGarage.Modules.PerformChecks.ViewModels;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SelectMaintenanceType
    /// </summary>
    public partial class SelectMaintenanceType : UserControl
    {
        public SelectMaintenanceType()
        {
            InitializeComponent();
            var viewModel = DataContext as SelectMaintenanceTypeViewModel;
            viewModel.ClosingRequest += (sender, e) => ((Window)Parent).Close();
        }
    }
}   //ImprezGarage.Modules.PerformChecks.Views namespace 