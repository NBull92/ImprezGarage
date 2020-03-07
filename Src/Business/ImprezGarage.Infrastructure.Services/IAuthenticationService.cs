namespace ImprezGarage.Infrastructure.Services
{
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task<string> CreateAccountAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
        void SignIn();
        string CurrentUser();
    }
}
