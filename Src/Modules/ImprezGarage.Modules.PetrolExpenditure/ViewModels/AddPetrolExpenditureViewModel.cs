//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;

    public class AddPetrolExpenditureViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        private const string EXPENSE_ADDED = "Expense added successfully!";
        private const string EXPENSE_FAILED = "An error occured during the adding of the petrol expense. \nPlease try again.";

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
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand<KeyEventArgs> OnKeyUpCommand { get; private set; }
        public DelegateCommand<KeyEventArgs> OnKeyDownCommand { get; private set; }
        #endregion
        #endregion

        #region Methods
        public AddPetrolExpenditureViewModel(IDataService dataService, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

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
            AddEnabled = string.IsNullOrEmpty(((TextBox)args.Source).Text) ? false : true;
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
                    _dialogService.Alert(EXPENSE_FAILED);                    
                    return;
                }

                _dialogService.Alert(EXPENSE_ADDED);
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
            if (this.ClosingRequest != null)
            {
                this.ClosingRequest(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 