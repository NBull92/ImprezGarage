//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using ImprezGarage.Infrastructure.BaseClasses;
    using Prism.Commands;

    public class ConfirmViewModel : DialogViewModelBase
    {
        #region Attributes
        private string _message;
        private string _header;
        #endregion

        #region Properties
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        #region Commands
        public DelegateCommand Confirm { get; set; }
        public DelegateCommand Cancel { get; set; }
        #endregion
        #endregion

        #region Methods
        public ConfirmViewModel()
        {
            Header = "Confirm!";
            Message = "This is an alert notification.";

            Confirm = new DelegateCommand(OnConfirm);
            Cancel = new DelegateCommand(Close);
        }

        #region Command Handlers
        private void OnConfirm()
        {
            Close(true);
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 