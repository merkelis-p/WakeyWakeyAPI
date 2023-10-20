using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(wakeyContext context) : base(context)
        {
        } 
    }
}