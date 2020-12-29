using Frontend.Models;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IUserService
    {
        public Task<User> LoginAsync(User user);
        public Task<User> RegisterUserAsync(RegisterUser user);
    }
}
