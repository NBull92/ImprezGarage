//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.ViewModels
{
    using Prism.Mvvm;

    public class ViewAViewModel : BindableBase
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewAViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}   //ImprezGarage.Modules.Logger.ViewModels namespace 