//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.StatusBar.ViewModels
{
    using ImprezGarage.Infrastructure;
    using Prism.Events;
    using Prism.Mvvm;

    public class StatusBarViewModel : BindableBase
    {
        #region Attributes
        private string _message;
        #endregion

        #region Proeprties
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        #endregion

        #region Methods
        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(Refresh);
            Message = "Ready";
        }

        private void Refresh()
        {
            Message = "All data up to date.";
        }
        #endregion
    }
}   //ImprezGarage.Modules.StatusBar.ViewModels namespace 