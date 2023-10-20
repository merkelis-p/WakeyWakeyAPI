using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{

    public class EventController : Repository<Event>
    {
        public EventController(wakeyContext context) : base(context)
        {
        }
    }

}