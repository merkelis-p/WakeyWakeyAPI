using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class ReminderController : GenericController<Reminder, ReminderRepository>, IReminderController
    {
        ReminderRepository _context;

        public ReminderController(ReminderRepository context) : base(context)
        {
            _context = context;
            
        }
        
    }
}