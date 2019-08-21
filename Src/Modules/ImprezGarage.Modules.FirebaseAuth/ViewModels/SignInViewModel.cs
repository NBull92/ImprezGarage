
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


    public class SignInViewModel : AuthenticateViewModel
    {
        #region Attributes
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        #endregion

        #region Commands
        public DelegateCommand SignIn { get; }
        public DelegateCommand ForgotPassword { get; }
        public DelegateCommand CreateAccount { get; }
        #endregion

        #region Methods
        public SignInViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base(eventAggregator, regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            SignIn = new DelegateCommand(OnSignIn);
            ForgotPassword = new DelegateCommand(OnForgotPassword);
            CreateAccount = new DelegateCommand(OnCreateAccount);
        }

        private void OnCreateAccount()
        {
            _regionManager.RequestNavigate(RegionNames.AuthenticateRegion, typeof(CreateAccount).FullName);
        }

        private void OnForgotPassword()
        {
           // throw new NotImplementedException();
        }

        private async void OnSignIn()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                return;
            
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
                var response = await auth.SignInWithEmailAndPasswordAsync(Email, Password);
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
        #endregion
    }
}
