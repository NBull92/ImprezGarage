//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using System;
    using System.Globalization;
    using System.Windows.Input;

    public class AddPetrolExpenditureViewModel : DialogViewModelBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationsService _notificationService;
        private readonly ILoggerService _loggerService;

        private const string ExpenseAdded = "Expense added successfully!";
        private const string ExpenseFailed = "An error occured during the adding of the petrol expense. \nPlease try again.";

        private double _amount;
        private DateTime? _date;
        private bool _addEnabled;
        #endregion

        #region Properties
        public double Amount
        {
            get => _amount;
            set
            {
               SetProperty(ref _amount, value);

                AddEnabled = !string.IsNullOrEmpty(_amount.ToString(CultureInfo.InvariantCulture));

                CanExecute();
            }
        }

        public DateTime? Date
        {
            get => _date;
            set
            {
                SetProperty(ref _date, value);
                CanExecute();
            }
        }

        public bool AddEnabled
        {
            get => _addEnabled;
            set => SetProperty(ref _addEnabled, value);
        }

        public int VehicleId { get; internal set; }

        #region Commands
        public DelegateCommand AddCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<KeyEventArgs> OnKeyDownCommand { get; }
        #endregion
        #endregion

        #region Methods
        public AddPetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator, INotificationsService notificationService,
            ILoggerService loggerService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _loggerService = loggerService;

            Date = DateTime.Now;

            AddCommand = new DelegateCommand(AddExecute).ObservesCanExecute(() => AddEnabled);
            CancelCommand = new DelegateCommand(Close);
            OnKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnKeyDownExecute);
        }

        #region Methods
        #region Command Handlers
        /// <summary>
        /// Proceed with adding the expenditure.
        /// Then close the window.
        /// </summary>
        private void AddExecute()
        {
            try
            {
                _dataService.AddPetrolExpenditure(Amount, Convert.ToDateTime(Date), VehicleId);
                _notificationService.Alert(ExpenseAdded);
                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();

            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
                _notificationService.Alert(ExpenseFailed);
            }
            Close();
        }
        #endregion
        
        /// <summary>
        /// Do a check on the amount and the date to see if the user has entered any data and can therefore proceed with saving this to the database.
        /// </summary>
        private void CanExecute()
        {
            if (Math.Abs(_amount) < 0.01 || Date == null)
            {
                AddEnabled = false;
            }
            else
            {
                AddEnabled = true;
            }
        }

        /// <summary>
        /// Check if the key is a numeric key by passing the Key to the KeyConverter.
        /// If it is a numpad key, then replace the text 'NumPad' from the string to leave only the number.
        /// Also check if either of the period/decimal/full stop buttons are pressed.
        /// Handle the input if it is not a numeric key.
        /// </summary>
        private void OnKeyDownExecute(KeyEventArgs args)
        {
            var strKey = new KeyConverter().ConvertToString(args.Key);
            strKey = strKey.Replace("NumPad", "");
            if (!int.TryParse(strKey, out int n) && strKey != "OemPeriod" && strKey != "Decimal")
                args.Handled = true;
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 