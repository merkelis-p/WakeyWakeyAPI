using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class ReminderController : GenericController<Reminder, ReminderRepository>
    {
        public ReminderController(ReminderRepository context) : base(context)
        {
            
        }
        
    }
}