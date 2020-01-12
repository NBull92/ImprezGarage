using Firebase.Auth;
using ImprezGarage.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;

namespace ImprezGarage.Modules.FirebaseAuth
{
    public static class FirebaseAuthExceptionHelper
    {
        public static string GetErrorReason(FirebaseAuthException exception)
        {
            switch (exception.Reason.ToString())
            {
                case "InvalidEmailAddress":
                    return "Invalid Email Address";
                case "WeakPassword":
                    return "Weak Password: A should be at least 6 characters";
                case "WrongPassword":
                    return "Wrong Password";
                default:
                    var error = "An undefined error occured while attempting to sign in.";
                    ServiceLocator.Current.GetInstance<ILoggerService>().LogException(exception, error);
                    return error;
            }
        }
    }
}
