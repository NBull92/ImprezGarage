namespace ImprezGarage.Modules.PetrolExpenditure.Views
{
    using ImprezGarage.Modules.PetrolExpenditure.ViewModels;
    using System.Windows;
    
    /// <summary>
    /// Interaction logic for AddPetrolExpenditure.xaml
    /// </summary>
    public partial class AddPetrolExpenditure : Window
    {
        public AddPetrolExpenditure()
        {
            InitializeComponent();
            var viewModel = DataContext as AddPetrolExpenditureViewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}