
namespace ImprezGarage.Modules.Account.ViewModels
{
    using Infrastructure;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public class AccountControlButtonViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

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
        
        public AccountControlButtonViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            UserName = "Demo";
            UserInitial = "I";

            SignOut = new DelegateCommand(OnSignOut);
            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }

        private void OnUserAccountChange(Tuple<bool, Infrastructure.Model.Account> loginData)
        {
            IsSignedIn = loginData.Item1;
            if (IsSignedIn)
            {
                UserName = loginData.Item2.Name;
                UserInitial = loginData.Item2.Name[0].ToString();
            }
            else
            {
                UserName = string.Empty;
                UserInitial = string.Empty;
            }
        }

        private void OnSignOut()
        {
            _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Infrastructure.Model.Account>(false, null));
        }

    }
}
