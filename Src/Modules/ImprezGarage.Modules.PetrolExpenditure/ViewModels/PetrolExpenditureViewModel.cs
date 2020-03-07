//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PetrolExpenditureViewModel : BindableBase, IDisposable
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IVehicleService _vehicleService;
        private readonly ObservableCollection<PetrolExpenseViewModel> _expenses;
        private string _label;
        private DateTime _fromDate;
        private DateTime _toDate;
        #endregion

        #region Properties
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private ObservableCollection<PetrolExpenseViewModel> _filteredExpenses;
        public ObservableCollection<PetrolExpenseViewModel> FilteredExpenses
        {
            get => _filteredExpenses;
            set => SetProperty(ref _filteredExpenses, value);
        }

        private string _expenseTotal;
        public string ExpenseTotal
        {
            get => _expenseTotal;
            set => SetProperty(ref _expenseTotal, value);
        }

        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        #endregion

        #region Methods
        public PetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator, IVehicleService vehicleService)
        {
            _dataService = dataService;
            _vehicleService = vehicleService;

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
            eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>().Subscribe(OnFilteredDatesChanged);

            _expenses = new ObservableCollection<PetrolExpenseViewModel>();
            FilteredExpenses = new ObservableCollection<PetrolExpenseViewModel>();
            ExpenseTotal = "£0.00";
        }

        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            if (vehicle == null)
            {
                FilteredExpenses.Clear();
                SelectedVehicle = null;
                return;
            }

            SelectedVehicle = vehicle;

            Label = _selectedVehicle.Make + " " + _selectedVehicle.Model;
            GetSelectedVehiclePetrolExpenses();
        }

        private void OnFilteredDatesChanged(Tuple<DateTime, DateTime> updatedDates)
        {
            _fromDate = updatedDates.Item1;
            _toDate = updatedDates.Item2;
            FilterExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            FilteredExpenses.Clear();

            if (_selectedVehicle == null)
                return;

            var expenses = _dataService.GetPetrolExpensesByVehicleId(_selectedVehicle.Id);

            if (expenses?.Result == null)
                return;
                
            foreach (var expense in expenses.Result)
            {
                var viewModel = ServiceLocator.Current.GetInstance<PetrolExpenseViewModel>();
                viewModel.LoadInstance(expense);
                _expenses.Add(viewModel);
            }

        }

        private void FilterExpenses()
        {
            FilteredExpenses.Clear();
            ExpenseTotal = "£0.00";

            if (_expenses == null)
                return;

            FilteredExpenses = new ObservableCollection<PetrolExpenseViewModel>(_expenses.Where(o => Convert.ToDateTime(o.DateEntered).Date >= _fromDate
                                                                                        && Convert.ToDateTime(o.DateEntered).Date <= _toDate)
                                                                                        .OrderBy(o => Convert.ToDateTime(o.DateEntered).Date));

            ExpenseTotal = $"£{FilteredExpenses.Select(o => o.Amount).Sum()}";
        }
        #endregion

        public void Dispose()
        {
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
        }
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 