//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;

    public class ConfirmViewModel : BindableBase
    {
        #region Attributes
        private string _message;
        private string _header;
        public event EventHandler ClosingRequest;
        #endregion

        #region Properties
        public bool DialogResult { get; set; }

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
            Header = "Alert!";
            Message = "This is an alert notification.";

            Confirm = new DelegateCommand(OnConfirm);
            Cancel = new DelegateCommand(OnCancel);
        }

        #region Command Handlers
        private void OnCancel()
        {
            DialogResult = false;
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnConfirm()
        {
            DialogResult = true;
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 