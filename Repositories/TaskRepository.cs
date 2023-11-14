using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;
using Task = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Repositories
{
    public class TaskRepository : Repository<Task>
    {
        public TaskRepository(wakeyContext context, ILogger<TaskRepository> logger) : base(context, logger)
        {

        }
        
        // Get by user id
        public async Task<Task> GetByUserIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == id);
        }
        
        // Get by subject id
        public async Task<Task> GetBySubjectIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.SubjectId == id);
        }
        
        // Get children by parent id
        public async Task<Task> GetChildrenByParentIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.ParentId == id);
        }
        
    }
}