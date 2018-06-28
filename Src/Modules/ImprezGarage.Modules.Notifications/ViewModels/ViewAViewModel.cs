//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using Prism.Mvvm;

    public class ViewAViewModel : BindableBase
    {
        #region Attributes
        private string _message;
        #endregion

        #region Properties
        public string Message
        {
            get => _message; 
            set => SetProperty(ref _message, value); 
        }
        #endregion

        #region Methods
        public ViewAViewModel()
        {
            Message = "View A from your Prism Module";
        }
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 