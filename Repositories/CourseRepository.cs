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
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(wakeyContext context, ILogger<CourseRepository> logger) : base(context, logger)
        {

        }

        public async Task<List<Course>> GetByUserIdAsync(int id)
        {
            return await _context.Courses.Where(c => c.UserId == id).ToListAsync();
            
        }
        
        public async Task<List<Course>> GetAllHierarchyAsync(int userId)
        {
            var courses = await _context.Courses
                .Where(c => c.UserId == userId)
                .Include(c => c.Subjects)
                .ThenInclude(s => s.Tasks.Where(t => t.ParentId == null))
                .AsNoTracking()
                .ToListAsync();

            foreach (var course in courses)
            {
                foreach (var subject in course.Subjects)
                {
                    foreach (var task in subject.Tasks)
                    {
                        task.SubTasks = await GetSubTasksAsync(task.Id);
                    }
                }
            }

            return courses;
        }

        public async Task<ICollection<Task>> GetSubTasksAsync(int taskId)
        {
            var subTasks = await _context.Tasks
                .Where(t => t.ParentId == taskId)
                .ToListAsync();

            foreach (var subTask in subTasks)
            {
                subTask.SubTasks = await GetSubTasksAsync(subTask.Id);
            }

            return subTasks;
        }

        
    }
}