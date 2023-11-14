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
        
        public async Task<bool> ExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
        
        // Exists Email
        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

    }

}