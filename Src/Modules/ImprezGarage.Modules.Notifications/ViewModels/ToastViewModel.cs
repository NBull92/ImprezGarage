//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;

    public class ToastViewModel : BindableBase
    {
        #region Attributes
        private string _message;
        private string _header;
        private string _buttonContent;

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

        public string ButtonContent
        {
            get => _buttonContent;
            set => SetProperty(ref _buttonContent, value);
        }

        #region Command
        public DelegateCommand Proceed { get; set; }       
        #endregion
        #endregion

        #region Methods
        public ToastViewModel()
        {
            Proceed = new DelegateCommand(OnProceed);
        }

        #region Command Handlers
        private void OnProceed()
        {
            DialogResult = true;
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 