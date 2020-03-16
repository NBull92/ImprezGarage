//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Media.Imaging;

    public class MainWindowViewModel : DialogViewModelBase
    {
        #region Attibute
        /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region Properties
        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        private string _title = "Imprez Garage";
        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        private bool _isSettingsOpen;
        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        private bool _isBusy = true;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        
        #region Commands
        /// <summary>
        /// Command for refreshing the current data.
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// Command for showing and closing the settings view.
        /// </summary>
        public DelegateCommand OpenSettings { get; set; }
        public DelegateCommand SignOut { get; set; }
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
            OpenSettings = new DelegateCommand(OnOpenSettings);
            SignOut = new DelegateCommand(OnSignOut);

            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }
        
        private void OnUserAccountChange(Tuple<bool, string> loginData)
        {
            IsBusy = !loginData.Item1;
            if (IsBusy)
            {
                ServiceLocator.Current.GetInstance<IAuthenticationService>().SignIn();
            }
        }

        private void OnSignOut()
        {
            _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, string>(false, string.Empty));
        }
        
        /// <summary>
        /// When the settings command is clicked, set IsSettingsOpen to it's opposite.
        /// </summary>
        private void OnOpenSettings()
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