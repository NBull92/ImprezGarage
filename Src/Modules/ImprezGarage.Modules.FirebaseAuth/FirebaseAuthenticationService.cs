
namespace ImprezGarage.Modules.FirebaseAuth
{
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Regions;
    using System;
    using System.Security;
    using System.Threading.Tasks;
    using Views;

    internal class FirebaseAuthenticationService : IAuthenticationService
    {
        private readonly IDataService _dataService;

        private Account _currentUser;

        public FirebaseAuthenticationService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Account> CreateAccountAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            UpdateCurrentUserWithCreated(email, response);
            return _currentUser;
        }

        public async Task<Account> CreateAccountAsync(string email, SecureString password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.CreateUserWithEmailAndPasswordAsync(email, SecureHelper.SecureStringToString(password));
            UpdateCurrentUserWithCreated(email, response);
            return _currentUser;
        }

        public async Task<Account> LoginAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(email, password);
            UpdateCurrentUserWithSignIn(response);
            return _currentUser;
        }

        public async Task<Account> LoginAsync(string email, SecureString password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(email, SecureHelper.SecureStringToString(password));
            UpdateCurrentUserWithSignIn(response);

            return _currentUser;
        }

        public void SignIn()
        {
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.AuthenticateRegion, typeof(SignIn).FullName);
        }

        public Account CurrentUser()
        {
            if(_currentUser == null)
                throw new NullReferenceException("No user is currently signed in.");

            return _currentUser;
        }

        private void UpdateCurrentUserWithCreated(string email, FirebaseAuthLink response)
        {
            _currentUser = _dataService.CreateUser(response.User.LocalId, email);
            _currentUser.LastLogin = DateTime.Now;
            _dataService.UpdateUser(_currentUser);
        }

        private void UpdateCurrentUserWithSignIn(FirebaseAuthLink response)
        {
            _currentUser = _dataService.GetUser(response.User.LocalId);
            _currentUser.LastLogin = DateTime.Now;
            _dataService.UpdateUser(_currentUser);
        }

    }
}
