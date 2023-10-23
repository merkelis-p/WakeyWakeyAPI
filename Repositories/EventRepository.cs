using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(wakeyContext context, ILogger<EventRepository> logger) : base(context, logger)
        {
            
        } 
    }
}