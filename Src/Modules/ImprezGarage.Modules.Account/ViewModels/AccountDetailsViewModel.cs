
namespace ImprezGarage.Modules.Account.ViewModels
{
    using Infrastructure;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public class AccountDetailsViewModel : BindableBase
    {
        private bool _isSignedIn;
        public bool IsSignedIn
        {
            get => _isSignedIn;
            set => SetProperty(ref _isSignedIn, value);
        }

        public AccountDetailsViewModel(IEventAggregator eventAggregator)
        {
            // TODO Re-enable when account details and classes have been setup.
            //eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }

        private void OnUserAccountChange(Tuple<bool, string> loginData)
        {
            IsSignedIn = loginData.Item1;
        }

    }
}
