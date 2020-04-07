
namespace ImprezGarage.Infrastructure.Services
{
    using Model;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task<Account> CreateAccountAsync(string email, string password);
        Task<Account> LoginAsync(string email, string password);
        void SignIn();
        Account CurrentUser();
    }
}
