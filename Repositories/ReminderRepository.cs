using System.Diagnostics;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class ReminderRepository : Repository<Reminder>
    {
        public ReminderRepository(wakeyContext context) : base(context)
        {
            
        }
        
    }
}