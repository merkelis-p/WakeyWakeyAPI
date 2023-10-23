using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class TaskRepository : Repository<Task>
    {
        public TaskRepository(wakeyContext context, ILogger<TaskRepository> logger) : base(context, logger)
        {
            
        }
    }
}