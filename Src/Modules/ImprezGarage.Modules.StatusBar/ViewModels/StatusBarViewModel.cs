//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.StatusBar.ViewModels
{
    using Infrastructure;
    using Prism.Events;
    using Prism.Mvvm;

    public class StatusBarViewModel : BindableBase
    {
        #region Attributes
        /// <summary>
        /// Store the message to be displayed on the view.
        /// </summary>
        private string _message;
        #endregion

        #region Proeprties
        /// <summary>
        /// Store the message to be displayed on the view.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for the view model and subscribe to the refresh event.
        /// </summary>
        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(Refresh);
            Message = "Ready";
        }

        /// <summary>
        /// Inform the user that the data is all up to date.
        /// </summary>
        private void Refresh()
        {
            Message = "All data up to date.";
        }
        #endregion
    }
}   //ImprezGarage.Modules.StatusBar.ViewModels namespace 