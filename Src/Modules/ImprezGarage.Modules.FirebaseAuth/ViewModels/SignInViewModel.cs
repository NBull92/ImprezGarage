//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.FirebaseAuth.ViewModels
{
    using Commands;
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Ioc;
    using Prism.Regions;
    using System;
    using Views;

    public class SignInViewModel : AuthenticateViewModel
    {
        #region Attributes
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Commands
        public DelegateCommand SignIn { get; }
        public DelegateCommand ForgotPassword { get; }
        public DelegateCommand CreateAccount { get; }
        public DemoAccountCommand DemoAccountCommand { get; set; }
        #endregion

        #region Methods
        public SignInViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IAuthenticationService authenticationService) : base(eventAggregator, regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _authenticationService = authenticationService;

            SignIn = new DelegateCommand(OnSignIn);
            ForgotPassword = new DelegateCommand(OnForgotPassword);
            CreateAccount = new DelegateCommand(OnCreateAccount);
            DemoAccountCommand = new DemoAccountCommand(authenticationService);
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
            if (string.IsNullOrWhiteSpace(Email) || SecurePassword.Length == 0)
                return;
            
            try
            {
                var response = await _authenticationService.LoginAsync(Email, SecurePassword);
                _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(true, response));
            }
            catch (FirebaseAuthException fae)
            {
                Error = FirebaseAuthExceptionHelper.GetErrorReason(fae);
            }
            catch (Exception e)
            {
                var logger = ContainerLocator.Current.Resolve(typeof(ILoggerService)) as ILoggerService;
                logger?.LogException(e, "Error occured while attempting to sign in.");
            }
        }
        #endregion
    }
}   // ImprezGarage.Modules.FirebaseAuth.ViewModels namespace 
