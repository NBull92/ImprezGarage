
using ImprezGarage.Modules.FirebaseAuth.Commands;

namespace ImprezGarage.Modules.FirebaseAuth.ViewModels
{
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using Views;
    
    public class CreateAccountViewModel : AuthenticateViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private string _rePassword;
        public string RePassword
        {
            get => _rePassword;
            set => SetProperty(ref _rePassword, value);
        }

        public DelegateCommand Register { get; }  
        public DelegateCommand SignIn { get; }
        public DemoAccountCommand DemoAccountCommand { get; set; }


        public CreateAccountViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base(eventAggregator, regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            Register = new DelegateCommand(OnRegisterAsync);
            SignIn = new DelegateCommand(OnSignIn);
            DemoAccountCommand = new DemoAccountCommand();
        }

        private void OnSignIn()
        {
            _regionManager.RequestNavigate(RegionNames.AuthenticateRegion, typeof(SignIn).FullName);
        }

        private async void OnRegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword))
                return;

            if (Password != RePassword)
            {
                Error = "The passwords do not match.";
                return;
            }

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
                var response = await auth.CreateUserWithEmailAndPasswordAsync(Email, Password);
                _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, string>(true, response.User.LocalId));
            }
            catch (FirebaseAuthException fae)
            {
                Error = FirebaseAuthExceptionHelper.GetErrorReason(fae);
            }
            catch (Exception e)
            {
                ServiceLocator.Current.GetInstance<ILoggerService>().LogException(e, "Error occured while attempting to sign in.");
            }
        }
    }
}
