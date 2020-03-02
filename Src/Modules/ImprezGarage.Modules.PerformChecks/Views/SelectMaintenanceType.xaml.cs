//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.Views
{
    using ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for SelectMaintenanceType
    /// </summary>
    public partial class SelectMaintenanceType : MetroWindow
    {
        public SelectMaintenanceType()
        {
            InitializeComponent();
            var viewModel = DataContext as SelectMaintenanceTypeViewModel;
            viewModel.CloseRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.PerformChecks.Views namespace 