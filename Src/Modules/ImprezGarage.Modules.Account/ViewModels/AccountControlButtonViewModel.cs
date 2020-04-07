
namespace ImprezGarage.Modules.Account.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using Views;

    public class AccountControlButtonViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _userInitial;
        public string UserInitial
        {
            get => _userInitial;
            set => SetProperty(ref _userInitial, value);
        }

        private bool _isSignedIn;
        public bool IsSignedIn
        {
            get => _isSignedIn;
            set => SetProperty(ref _isSignedIn, value);
        }

        public DelegateCommand SignOut { get; set; }
        public DelegateCommand ViewProfile { get; set; }
        
        public AccountControlButtonViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            UserName = "Demo";
            UserInitial = "I";

            SignOut = new DelegateCommand(OnSignOut);
            ViewProfile = new DelegateCommand(OnViewProfile);
            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }

        private void OnViewProfile()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ProfilePage).FullName);
            _regionManager.RequestNavigate(RegionNames.VehicleHeaderRegion, typeof(ProfileHeader).FullName);
        }

        private void OnUserAccountChange(Tuple<bool, Account> loginData)
        {
            IsSignedIn = loginData.Item1;
            if (IsSignedIn)
            {
                UserName = loginData.Item2.Name;
                UserInitial = loginData.Item2.Name[0].ToString().ToUpper();
            }
            else
            {
                UserName = string.Empty;
                UserInitial = string.Empty;
            }
        }

        private void OnSignOut()
        {
            _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(false, null));
        }

    }
}
