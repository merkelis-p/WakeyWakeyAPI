using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using WakeyWakeyAPI.Models;
using Task = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(wakeyContext context, ILogger<CourseRepository> logger) : base(context, logger)
        {

        }
        
        // Get by user id
        public async Task<Course> GetByUserIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.UserId == id);
        }
        
        
        // Get all hierarchy
        public async Task<Course> GetAllHierarchyAsync(int userId)
        {
            // Use eager loading to include related subjects and their tasks
            var course = await _context.Courses
                .Where(c => c.UserId == userId)
                .Include(c => c.Subjects)
                .ThenInclude(s => s.Tasks.Where(t => t.ParentId == null))
                .AsNoTracking() // Use AsNoTracking if you're only reading data for better performance
                .FirstOrDefaultAsync();

            // Now load subtasks for each task
            if (course != null)
            {
                foreach (var subject in course.Subjects)
                {
                    foreach (var task in subject.Tasks)
                    {
                        // Recursively load subtasks
                        task.SubTasks = await GetSubTasksAsync(task.Id);
                    }
                }
            }

            return course;
        }

        private async Task<ICollection<Task>> GetSubTasksAsync(int taskId)
        {
            var subTasks = await _context.Tasks
                .Where(t => t.ParentId == taskId)
                .ToListAsync();

            foreach (var subTask in subTasks)
            {
                // Recursively load subtasks
                subTask.SubTasks = await GetSubTasksAsync(subTask.Id);
            }

            return subTasks;
        }

        
    }
}