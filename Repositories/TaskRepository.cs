using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class TaskRepository : Repository<Task>
    {
        public TaskRepository(wakeyContext context) : base(context)
        {
            
        }
    }
}