namespace ImprezGarage.Infrastructure.Services
{
    public interface IAuthenticationService
    {
        string CreateAccount(string email, string password);
        string Login(string email, string password);
        void SignIn();
        string CurrentUser();
    }
}
