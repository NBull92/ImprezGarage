//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.ViewModels
{
    using Infrastructure;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        #region Attibute
        private readonly IEventAggregator _eventAggregator;
        private string _title = "Imprez Garage";
        private bool _isSettingsOpen;
        #endregion

        #region Properties
        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        #region Command
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand Settings { get; set; }
        #endregion
        #endregion

        #region Methods
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            RefreshCommand = new DelegateCommand(RefreshExecute);
            Settings = new DelegateCommand(OnSettingsClicked);
        }

        private void OnSettingsClicked()
        {
            IsSettingsOpen = !IsSettingsOpen;
        }

        private void RefreshExecute()
        {
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
        }
        #endregion
    }
}   //ImprezGarage.ViewModels namespace 