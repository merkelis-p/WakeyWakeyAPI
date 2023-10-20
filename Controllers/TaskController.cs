using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class TaskController: GenericController<Task, TaskRepository>
    {
        
        TaskRepository _context;
        public TaskController(TaskRepository context) : base(context)
        {
            _context = context;
        }
        
    }
}