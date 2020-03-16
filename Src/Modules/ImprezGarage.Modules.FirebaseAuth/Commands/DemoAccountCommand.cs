
namespace ImprezGarage.Modules.FirebaseAuth.Commands
{
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Events;
    using System;
    using System.Windows.Input;

    public class DemoAccountCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var authService = ServiceLocator.Current.GetInstance<IAuthenticationService>();
            var response = await authService.LoginAsync(DemoAccountDetails.Email, DemoAccountDetails.Password);
            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, Account>(true, response));
        }
    }
}
