using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class TaskController: GenericController<Task, TaskRepository>
    {
        public TaskController(TaskRepository context) : base(context)
        {
            
        }
        
    }
}