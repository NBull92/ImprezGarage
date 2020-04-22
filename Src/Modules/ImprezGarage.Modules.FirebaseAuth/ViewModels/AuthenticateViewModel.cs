
using System.Security;

namespace ImprezGarage.Modules.FirebaseAuth.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Windows;


    public class AuthenticateViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private SecureString _securePassword;
        public SecureString SecurePassword
        {
            get => _securePassword;
            set => SetProperty(ref _securePassword, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _error;
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        public DelegateCommand Cancel { get; set; }

        public AuthenticateViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            Cancel = new DelegateCommand(OnCancel);
        }

        private void OnCancel()
        {
            _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(false, null));
            Application.Current.Shutdown();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (_regionManager.Regions[RegionNames.AuthenticateRegion].Views.Contains(this))
                _regionManager.Regions[RegionNames.AuthenticateRegion].Remove(this);
        }
    }
}
