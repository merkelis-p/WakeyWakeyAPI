using System.Runtime.InteropServices.ComTypes;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class EventController : GenericController<Event, EventRepository>
    {
        EventRepository _context;
        public EventController(EventRepository context) : base(context)
        {
            _context = context;
        }
    }

}