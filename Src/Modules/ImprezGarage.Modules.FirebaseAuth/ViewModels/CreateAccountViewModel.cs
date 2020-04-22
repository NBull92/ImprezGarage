﻿
using System.Security;

namespace ImprezGarage.Modules.FirebaseAuth.ViewModels
{
    using Commands;
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Model;
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

        private SecureString _rePassword;
        public SecureString RePassword
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
            if (string.IsNullOrWhiteSpace(Email) || SecurePassword.Length == 0 || RePassword.Length == 0)
                return;

            if (SecureHelper.SecureStringEqual(SecurePassword, RePassword))
            {
                Error = "The passwords do not match.";
                return;
            }

            try
            {
                var authService = ServiceLocator.Current.GetInstance<IAuthenticationService>();
                var response = await authService.CreateAccountAsync(Email, SecurePassword);
                _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(true, response));
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
