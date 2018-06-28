//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using ImprezGarage.Modules.PetrolExpenditure.Views;
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
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<PetrolExpenseViewModel> _expenses;
        private VehicleViewModel _selectedVehicle;
        #endregion

        #region Properties
        public ObservableCollection<PetrolExpenseViewModel> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public DelegateCommand AddExpenditureCommand { get;set; }
        #endregion

        #region Methods
        public PetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);

            AddExpenditureCommand = new DelegateCommand(AddExpenditureExecute);
        }

        #region Command Handlers

        private void AddExpenditureExecute()
        {
            var addExpense = new AddPetrolExpenditure();
            var vm = addExpense.DataContext as AddPetrolExpenditureViewModel;
            vm.VehicleId = _selectedVehicle.Vehicle.Id;
            addExpense.ShowDialog();
        }
        #endregion

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            _selectedVehicle = vehicleViewModel;
            GetSelectedVehiclePetrolExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            if (_selectedVehicle == null)
            {
                Expenses = null;
            }
            else
            {
                _dataService.GetPetrolExpensesByVehicleId((expenses, error) =>
                {
                    if (error != null)
                    {

                    }

                    Expenses = new ObservableCollection<PetrolExpenseViewModel>();

                    foreach (var expense in expenses)
                    {
                        var viewModel = ServiceLocator.Current.GetInstance<PetrolExpenseViewModel>();
                        viewModel.LoadInstance(expense);
                        Expenses.Add(viewModel);
                    }

                    Expenses = new ObservableCollection<PetrolExpenseViewModel>(Expenses.OrderByDescending(o => o.DateEntered));
                }, _selectedVehicle.Vehicle.Id);
            }
        }
        #endregion
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 