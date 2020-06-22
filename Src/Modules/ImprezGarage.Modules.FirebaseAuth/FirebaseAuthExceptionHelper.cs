//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.FirebaseAuth
{
    using CommonServiceLocator;
    using Firebase.Auth;
    using Infrastructure.Services;

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
}   // ImprezGarage.Modules.FirebaseAuth namespace 