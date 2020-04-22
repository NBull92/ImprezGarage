
namespace ImprezGarage.Modules.FirebaseAuth
{
    using Firebase.Auth;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Regions;
    using System;
    using System.Runtime.InteropServices;
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
            _currentUser = _dataService.CreateUser(response.User.LocalId, email);
            return _currentUser;
        }

        public async Task<Account> CreateAccountAsync(string email, SecureString password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.CreateUserWithEmailAndPasswordAsync(email, SecureHelper.SecureStringToString(password));
            _currentUser = _dataService.CreateUser(response.User.LocalId, email);
            return _currentUser;
        }

        public async Task<Account> LoginAsync(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(email, password);
            _currentUser = _dataService.GetUser(response.User.LocalId);
            return _currentUser;
        }

        public async Task<Account> LoginAsync(string email, SecureString password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseProjectConfig.ApiKey));
            var response = await auth.SignInWithEmailAndPasswordAsync(email, SecureHelper.SecureStringToString(password));
            _currentUser = _dataService.GetUser(response.User.LocalId);
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
    }

    public static class SecureHelper
    {
        public static string SecureStringToString(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static bool SecureStringEqual(SecureString secureString1, SecureString secureString2)
        {
            if (secureString1 == null)
            {
                throw new ArgumentNullException("s1");
            }
            if (secureString2 == null)
            {
                throw new ArgumentNullException("s2");
            }

            if (secureString1.Length != secureString2.Length)
            {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try
            {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(secureString1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(secureString2);

                String str1 = Marshal.PtrToStringBSTR(ss_bstr1_ptr);
                String str2 = Marshal.PtrToStringBSTR(ss_bstr2_ptr);

                return str1.Equals(str2);
            }
            finally
            {
                if (ss_bstr1_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }
    }
}
