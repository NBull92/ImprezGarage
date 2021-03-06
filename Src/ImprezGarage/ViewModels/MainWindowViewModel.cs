﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using Prism.Regions;
using Unity;

namespace ImprezGarage.ViewModels
{
    using CommonServiceLocator;
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public class MainWindowViewModel : DialogViewModelBase
    {
        #region Attibute
        /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        private readonly IUnityContainer _unityContainer;

        #endregion

        #region Properties
        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        private string _title = "ImprezGarage";
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
        
        public string Version { get; set; }

        #region Commands
        /// <summary>
        /// Command for refreshing the current data.
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// Command for showing and closing the settings view.
        /// </summary>
        public DelegateCommand OpenSettings { get; set; }

        public DelegateCommand OpenHelp { get; set; }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Construct this view model and inject the event aggregator.
        /// </summary>
        public MainWindowViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _unityContainer = unityContainer;

            RefreshCommand = new DelegateCommand(RefreshExecute);
            OpenSettings = new DelegateCommand(OnOpenSettings);
            OpenHelp = new DelegateCommand(OnOpenHelp);

            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);

            GetVersion();
        }

        private void OnOpenHelp()
        {
            Process.Start("http://www.imprezgarage.com/contact");
        }

        /// <summary>
        /// Get the current version number of the app, for showing in the UI.
        /// No need for revision being listed for now.
        /// </summary>
        private void GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var major = version.Major;
            var minor = version.Minor;
            var build = version.Build;
            Version = $"v{major}.{minor}.{build}";
        }

        private void OnUserAccountChange(Tuple<bool, Account> loginData)
        {
            IsBusy = !loginData.Item1;
            if (IsBusy)
            {
                _unityContainer.Resolve<IAuthenticationService>().SignIn();
                //ServiceLocator.Current.GetInstance<IAuthenticationService>().SignIn();
            }
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