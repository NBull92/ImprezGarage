//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Services;
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
            if (vehicleViewModel == null)
            {
                Expenses = null;
                return;
            }

            _selectedVehicle = vehicleViewModel;

            Label = _selectedVehicle.Make + " " + _selectedVehicle.Model;
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
                Expenses = new ObservableCollection<PetrolExpenseViewModel>();

                var expenses = _dataService.GetPetrolExpensesByVehicleId(_selectedVehicle.Vehicle.Id);

                if (expenses == null || expenses.Result == null)
                    return;
                
                foreach (var expense in expenses.Result)
                {
                    var viewModel = ServiceLocator.Current.GetInstance<PetrolExpenseViewModel>();
                    viewModel.LoadInstance(expense);
                    Expenses.Add(viewModel);
                }

                Expenses = new ObservableCollection<PetrolExpenseViewModel>(Expenses.OrderByDescending(o => o.DateEntered));
            }
        }
        #endregion
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 