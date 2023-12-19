using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class EventController : GenericController<Event, EventRepository>, IEventController
    {
        EventRepository _context;
        public EventController(EventRepository context) : base(context)
        {
            _context = context;
        }
        
        
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Event>> GetByUserId(int id)
        {
            var ev = await _context.GetByUserIdAsync(id);
            if (ev == null || !ev.Any())
            {
                return NotFound();
            }

            return Ok(ev);
        }

        
    }
    
    
}