//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.Views
{
    using ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for AddPetrolExpenditure.xaml
    /// </summary>
    public partial class AddPetrolExpenditure : MetroWindow
    {
        public AddPetrolExpenditure()
        {
            InitializeComponent();
            var viewModel = DataContext as AddPetrolExpenditureViewModel;
            viewModel.CloseRequest += (sender, e) => Close();
        }
    }
}   //ImprezGarage.Modules.PetrolExpenditure.Views namespace 