using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class EventController : GenericController<Event, EventRepository>
    {
        public EventController(EventRepository context) : base(context)
        {
            
        }
    }

}