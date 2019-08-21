namespace ImprezGarage.Modules.FirebaseAuth
{
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Regions;
    using Views;

    public class FirebaseAuthenticationService : IAuthenticationService
    {
        private string _userId;

        public string CreateAccount(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            _userId = response.Result.User.LocalId;
            return _userId;
        }

        public string Login(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = auth.SignInWithEmailAndPasswordAsync(email, password);
            _userId = response.Result.User.LocalId;
            return _userId;
        }

        public void SignIn()
        {
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.AuthenticateRegion, typeof(SignIn).FullName);
        }
    }
}
