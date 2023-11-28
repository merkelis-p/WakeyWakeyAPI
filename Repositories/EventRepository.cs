using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(wakeyContext context, ILogger<EventRepository> logger) : base(context, logger)
        {
            
        } 
        
        // Get by user id
        public async Task<Event> GetByUserIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(c => c.UserId == id);
        }
        
    }
}