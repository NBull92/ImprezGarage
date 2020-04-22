
namespace ImprezGarage.Infrastructure.Services
{
    using Model;
    using System.Threading.Tasks;
    using System.Security;

    public interface IAuthenticationService
    {
        Task<Account> CreateAccountAsync(string email, string password);
        Task<Account> CreateAccountAsync(string email, SecureString password);
        Task<Account> LoginAsync(string email, string password);
        Task<Account> LoginAsync(string email, SecureString password);
        void SignIn();
        Account CurrentUser();
    }
}
