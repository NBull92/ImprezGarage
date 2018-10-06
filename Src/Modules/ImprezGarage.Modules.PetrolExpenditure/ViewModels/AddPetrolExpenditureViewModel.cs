//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class AddPetrolExpenditureViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationsService _notificationService;

        private const string ExpenseAdded = "Expense added successfully!";
        private const string ExpenseFailed = "An error occured during the adding of the petrol expense. \nPlease try again.";

        private double _amount = 0;
        private bool _addEnabled;
        public event EventHandler ClosingRequest;
        #endregion

        #region Properties
        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
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
        public DelegateCommand<KeyEventArgs> OnKeyUpCommand { get; }
        public DelegateCommand<KeyEventArgs> OnKeyDownCommand { get; }
        #endregion
        #endregion

        #region Methods
        public AddPetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator, INotificationsService notificationService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;

            AddCommand = new DelegateCommand(AddExecute);
            CancelCommand = new DelegateCommand(Close);
            OnKeyUpCommand = new DelegateCommand<KeyEventArgs>(OnKeyUpExecute);
            OnKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnKeyDownExecute);
        }

        #region Command Handlers
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

        /// <summary>
        /// Check if there was any text left in the textbox.
        /// Enable Add button if so.
        /// </summary>
        private void OnKeyUpExecute(KeyEventArgs args)
        {
            AddEnabled = !string.IsNullOrEmpty(((TextBox)args.Source).Text);
        }

        /// <summary>
        /// Proceed with adding the expenditure.
        /// Then close the window.
        /// </summary>
        private void AddExecute()
        {
            _dataService.AddPetrolExpenditure((error) =>
            {
                if(error != null)
                {
                    _notificationService.Alert(ExpenseFailed);
                    return;
                }

                _notificationService.Alert(ExpenseAdded);
                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
            }, Convert.ToDouble(Amount), VehicleId);
            Close();
        }
        #endregion
        
        /// <summary>
        /// Closes the window.
        /// </summary>
        private void Close()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 