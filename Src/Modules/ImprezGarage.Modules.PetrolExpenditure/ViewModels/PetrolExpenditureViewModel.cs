//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure;
    using Infrastructure.Services;
    using ImprezGarage.Infrastructure.ViewModels;
    using Views;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PetrolExpenditureViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private ObservableCollection<PetrolExpenseViewModel> _expenses;
        private VehicleViewModel _selectedVehicle;
        private string _label;
        #endregion

        #region Properties
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public ObservableCollection<PetrolExpenseViewModel> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public DelegateCommand AddExpenditureCommand { get;set; }
        #endregion

        #region Methods
        public PetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);

            AddExpenditureCommand = new DelegateCommand(AddExpenditureExecute);
            Expenses = new ObservableCollection<PetrolExpenseViewModel>();
        }

        #region Command Handlers

        private void AddExpenditureExecute()
        {
            var addExpense = new AddPetrolExpenditure();
            if (addExpense.DataContext is AddPetrolExpenditureViewModel vm)
                vm.VehicleId = _selectedVehicle.Vehicle.Id;
            addExpense.ShowDialog();
        }
        #endregion

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            if (vehicleViewModel == null)
            {
                Expenses.Clear();
                SelectedVehicle = null;
                return;
            }

            SelectedVehicle = vehicleViewModel;

            Label = _selectedVehicle.Make + " " + _selectedVehicle.Model;
            GetSelectedVehiclePetrolExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            Expenses.Clear();

            if (_selectedVehicle == null)
                return;

            var expenses = _dataService.GetPetrolExpensesByVehicleId(_selectedVehicle.Vehicle.Id);

            if (expenses?.Result == null)
                return;
                
            foreach (var expense in expenses.Result)
            {
                var viewModel = ServiceLocator.Current.GetInstance<PetrolExpenseViewModel>();
                viewModel.LoadInstance(expense);
                Expenses.Add(viewModel);
            }

            Expenses = new ObservableCollection<PetrolExpenseViewModel>(Expenses.OrderByDescending(o => o.DateEntered));
        }
        #endregion
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 