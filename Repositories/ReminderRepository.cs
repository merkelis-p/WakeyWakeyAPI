using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class ReminderRepository : Repository<Reminder>
    {
        public ReminderRepository(wakeyContext context, ILogger<ReminderRepository> logger) 
            : base(context, logger)
        {
        }
        
    }
}