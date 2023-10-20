using WakeyWakeyAPI.Models;



namespace WakeyWakeyAPI.Controllers
{

    public class EventsController : Repository<Event>
    {
        public EventsController(wakeyContext context) : base(context)
        {
        }
    }

}