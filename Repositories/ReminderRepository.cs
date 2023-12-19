using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        public ReminderRepository(wakeyContext context, ILogger<ReminderRepository> logger) 
            : base(context, logger)
        {
        }
        
    }
}