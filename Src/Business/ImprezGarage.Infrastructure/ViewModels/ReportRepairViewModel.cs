
namespace ImprezGarage.Infrastructure.ViewModels
{
    using BaseClasses;
    using Prism.Commands;
    using Services;
    using System;
    using System.Globalization;
    using System.Windows.Input;

    public class ReportRepairViewModel : DialogViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INotificationsService _notificationService;
        private readonly ILoggerService _loggerService;

        private const string ReportComplete = "Repair sucessfully reported!";
        private const string ReportFailed = "An error occured during the reporting of the repair. \nPlease try again.";

        private bool _addEnabled;
        public bool AddEnabled
        {
            get => _addEnabled;
            set => SetProperty(ref _addEnabled, value);
        }

        private string _part;
        public string Part
        {
            get => _part;
            set
            {
                SetProperty(ref _part, value);
                AllowAdd();
            }
        }

        private void AllowAdd()
        {
            if (string.IsNullOrWhiteSpace(Part))
            {
                AddEnabled = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(Replacement))
            {
                AddEnabled = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(Price.ToString(CultureInfo.InvariantCulture)))
            {
                AddEnabled = false;
                return;
            }
            if (Math.Abs(Price) < 0.01)
            {
                AddEnabled = false;
                return;
            }

            AddEnabled = true;
        }

        private string _replacement;
        public string Replacement
        {
            get => _replacement;
            set
            {
                SetProperty(ref _replacement, value);
                AllowAdd();
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                AllowAdd();
            }
        }

        public int VehicleId { get; internal set; }

        #region Commands
        public DelegateCommand Add { get; }
        public DelegateCommand Cancel { get; }
        public DelegateCommand<KeyEventArgs> OnKeyDownCommand { get; }
        #endregion

        public ReportRepairViewModel(IDataService dataService, INotificationsService notificationService, ILoggerService loggerService)
        {
            _dataService = dataService;
            _notificationService = notificationService;
            _loggerService = loggerService;

            Add = new DelegateCommand(OnAdd).ObservesCanExecute(() => AddEnabled);
            Cancel = new DelegateCommand(Close);
            OnKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnKeyDownExecute);
        }

        /// <summary>
        /// Proceed with adding the expenditure.
        /// Then close the window.
        /// </summary>
        private void OnAdd()
        {
            try
            {
                _dataService.AddRepairReport(Part, Replacement, Price, VehicleId);
                _notificationService.Alert(ReportComplete);
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
                _notificationService.Alert(ReportFailed);
            }
            Close();
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
    }
}
