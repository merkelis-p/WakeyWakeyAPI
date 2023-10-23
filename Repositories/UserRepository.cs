using System.Threading.Tasks;
using WakeyWakeyAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(wakeyContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

    }

}