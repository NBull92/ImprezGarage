//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using CommonServiceLocator;
    using CountriesWrapper;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PetrolExpenditureViewModel : BindableBase, IDisposable
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IVehicleService _vehicleService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICountryManager _countryManager;
        private ObservableCollection<PetrolExpenseViewModel> _expenses;
        private DateTime _fromDate;
        private DateTime _toDate;
        #endregion

        #region Properties
        private string _label;
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

        private double _expenseTotal;
        public double ExpenseTotal
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

        private string _currencyLabel;
        public string CurrencyLabel
        {
            get => _currencyLabel;
            set => SetProperty(ref _currencyLabel, value);
        }

        public DelegateCommand<PetrolExpenseViewModel> DeleteExpenseCommand { get; set; }
        #endregion

        #region Methods
        public PetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator, IVehicleService vehicleService, 
            IAuthenticationService authenticationService, ICountryManager countryManager)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _vehicleService = vehicleService;
            _authenticationService = authenticationService;
            _countryManager = countryManager;

            ResetParameters();

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;
            eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>().Subscribe(OnFilteredDatesChanged);
            eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);

            DeleteExpenseCommand = new DelegateCommand<PetrolExpenseViewModel>(OnExpenseDeleted);
        }

        private void OnExpenseDeleted(PetrolExpenseViewModel expense)
        {
            if (!expense.Delete())
                return;

            _expenses.Remove(_expenses.FirstOrDefault(o => o.Id.Equals(expense.Id)));
            FilterExpenses();
            _eventAggregator.GetEvent<PetrolEvents.ExpenseDeleted>().Publish(expense.Id);
        }

        private void ResetParameters()
        {
            _expenses = new ObservableCollection<PetrolExpenseViewModel>();
            FilteredExpenses = new ObservableCollection<PetrolExpenseViewModel>();
            ExpenseTotal = 0.00;
            _fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _toDate = DateTime.Now;
        }

        private void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            FilteredExpenses.Clear();
            _expenses.Clear();
            if (vehicle == null)
            {
                SelectedVehicle = null;
                return;
            }

            SelectedVehicle = vehicle;

            Label = _selectedVehicle.Make + " " + _selectedVehicle.Model;
            GetSelectedVehiclePetrolExpenses();
            FilterExpenses();
        }

        private void OnFilteredDatesChanged(Tuple<DateTime, DateTime> updatedDates)
        {
            _fromDate = updatedDates.Item1;
            _toDate = updatedDates.Item2;

            GetSelectedVehiclePetrolExpenses();
            FilterExpenses();

            var currentUser = _authenticationService.CurrentUser();
            if (currentUser != null)
            {
                GetCurrentUsersCurrencySymbol(currentUser.Country);
            }
            else
            {
                CurrencyLabel = string.Empty;
            }
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            _expenses.Clear();

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
            ExpenseTotal = 0.00;

            if (_expenses == null)
                return;

            FilteredExpenses = new ObservableCollection<PetrolExpenseViewModel>(_expenses.Where(o => Convert.ToDateTime(o.DateEntered).Date >= _fromDate
                                                                                        && Convert.ToDateTime(o.DateEntered).Date <= _toDate)
                                                                                        .OrderBy(o => Convert.ToDateTime(o.DateEntered).Date));

            ExpenseTotal = FilteredExpenses.Select(o => o.Amount).Sum();
        }

        private void OnUserAccountChange(Tuple<bool, Account> loginData)
        {
            var isSignedIn = loginData.Item1;
            if (isSignedIn)
            {
                GetCurrentUsersCurrencySymbol(loginData.Item2.Country);
            }
            else
            {
                CurrencyLabel = string.Empty;
            }
        }

        private void GetCurrentUsersCurrencySymbol(string countryName)
        {
            var country = _countryManager.GetCountry(countryName);
            if (country != null)
            {
                var currency = country.Currencies.FirstOrDefault();
                if (currency != null)
                    CurrencyLabel = currency.Symbol;
            }
            else
            {
                CurrencyLabel = string.Empty;
            }
        }

        public void Dispose()
        {
            _vehicleService.SelectedVehicleChanged -= OnSelectedVehicleChanged;
            _eventAggregator.GetEvent<PetrolEvents.FilteredDatesChanged>().Unsubscribe(OnFilteredDatesChanged);
            _eventAggregator.GetEvent<Events.UserAccountChange>().Unsubscribe(OnUserAccountChange);
        }
        #endregion
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 