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
        /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        private string _title = "Imprez Garage";

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        private bool _isSettingsOpen;
        #endregion

        #region Properties
        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        #region Command
        /// <summary>
        /// Command for refreshing the current data.
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// Command for showing and closing the settings view.
        /// </summary>
        public DelegateCommand Settings { get; set; }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Construct this view model and inject the event aggregator.
        /// </summary>
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            RefreshCommand = new DelegateCommand(RefreshExecute);
            Settings = new DelegateCommand(OnSettingsClicked);
        }

        /// <summary>
        /// When the settings command is clicked, set IsSettingsOpen to it's opposite.
        /// </summary>
        private void OnSettingsClicked()
        {
            IsSettingsOpen = !IsSettingsOpen;
        }

        /// <summary>
        /// When the user clicks the refresh, publish the refresh data event and that will call any subscribers.
        /// </summary>
        private void RefreshExecute()
        {
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
        }
        #endregion
    }
}   //ImprezGarage.ViewModels namespace 