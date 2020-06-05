//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.FirebaseAuth.Commands
{
    using CommonServiceLocator;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Events;
    using System;
    using System.Windows.Input;

    public class DemoAccountCommand : ICommand
    {
        private readonly IAuthenticationService _authenticationService;
        public event EventHandler CanExecuteChanged;

        public DemoAccountCommand(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var response = await _authenticationService.LoginAsync(DemoAccountDetails.Email, DemoAccountDetails.Password);
            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(true, response));
        }
    }
}   // ImprezGarage.Modules.FirebaseAuth.Commands namespace 