using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class ReminderController : GenericController<Reminder, IReminderRepository>, IReminderController
    {
        IReminderRepository _context;

        public ReminderController(IReminderRepository context) : base(context)
        {
            _context = context;
            
        }
        
    }
}