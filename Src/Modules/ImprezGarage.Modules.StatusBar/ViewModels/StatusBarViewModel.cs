//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.StatusBar.ViewModels
{
    using System;
    using ImprezGarage.Infrastructure;
    using Prism.Events;
    using Prism.Mvvm;

    public class StatusBarViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(Refresh);
            Message = "Ready";
        }

        private void Refresh()
        {
            Message = "All data up to date.";
        }
    }
}   //ImprezGarage.Modules.StatusBar.ViewModels namespace 