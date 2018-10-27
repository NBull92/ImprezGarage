//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using ImprezGarage.Infrastructure.BaseClasses;
    using Prism.Commands;

    public class AlertViewModel : DialogViewModelBase
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
        public DelegateCommand Okay { get;set;}
        #endregion
        #endregion

        #region Methods
        public AlertViewModel()
        {
            Header = "Alert!";
            Message = "This is an alert notification.";

            Okay = new DelegateCommand(Close);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 