//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.ViewModels
{
    using System;
    using ImprezGarage.Infrastructure;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private string _title = "Imprez Garage";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand RefreshCommand { get; set; }

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            RefreshCommand = new DelegateCommand(RefreshExecute);
        }

        private void RefreshExecute()
        {
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
        }
    }
}   //ImprezGarage.ViewModels namespace 