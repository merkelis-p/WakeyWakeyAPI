using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WakeyWakeyAPI.Repositories
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(wakeyContext context, ILogger<EventRepository> logger) : base(context, logger)
        {
            
        } 
        
        // Get by user id
        public async Task<IEnumerable<Event>> GetByUserIdAsync(int id)
        {
            return await _context.Events.Where(e => e.UserId == id).ToListAsync();
        }
        
    }
}