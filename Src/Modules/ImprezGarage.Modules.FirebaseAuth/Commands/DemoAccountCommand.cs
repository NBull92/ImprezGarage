using System;
using System.Windows.Input;
using Firebase.Auth;
using ImprezGarage.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;

namespace ImprezGarage.Modules.FirebaseAuth.Commands
{
    public class DemoAccountCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(DemoAccountDetails.Email, DemoAccountDetails.Password);
            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, string>(true, response.User.LocalId));
        }
    }
}
