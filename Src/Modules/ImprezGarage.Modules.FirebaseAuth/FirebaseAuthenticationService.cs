namespace ImprezGarage.Modules.FirebaseAuth
{
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Regions;
    using Views;
    using System;
    using System.Threading.Tasks;

    internal class FirebaseAuthenticationService : IAuthenticationService
    {
        private string _userId;

        public async Task<string> CreateAccountAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            _userId = response.User.LocalId;
            return _userId;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(email, password);
            _userId = response.User.LocalId;
            return _userId;
        }

        public void SignIn()
        {
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.AuthenticateRegion, typeof(SignIn).FullName);
        }

        public string CurrentUser()
        {
            if(string.IsNullOrEmpty(_userId))
                throw new NullReferenceException("No user is currently signed in.");

            return _userId;
        }
    }
}
