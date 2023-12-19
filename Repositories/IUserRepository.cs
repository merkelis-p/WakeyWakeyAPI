using System.Threading.Tasks;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public interface IUserRepository : IRepository<User>
    {

        public Task<User> GetByUsernameAsync(string username);
        public Task<bool> ExistsAsync(string username);
        public Task<bool> ExistsEmailAsync(string email);
        
    }
}