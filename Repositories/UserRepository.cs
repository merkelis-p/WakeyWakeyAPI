using System.Threading.Tasks;
using WakeyWakeyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WakeyWakeyAPI.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(wakeyContext context) : base(context)
        {
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

    }

}