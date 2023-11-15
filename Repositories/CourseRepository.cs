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
        public async Task<Course> GetAllHierarchyAsync(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.UserId == id);
            var subjects = await _context.Subjects.ToListAsync();
            var tasks = await _context.Tasks.ToListAsync();
            course.Subjects = subjects.FindAll(s => s.CourseId == course.Id);
            foreach (var subject in course.Subjects)
            {
                subject.Tasks = await _context.Tasks.Where(t => t.SubjectId == subject.Id && t.ParentId == null).ToListAsync();
                // recursive way to get all subtasks
                foreach (var task in subject.Tasks)
                {
                    task.SubTasks = GetSubTasks(task.Id, tasks);
                }
                
            }
            return course;
        }

        private ICollection<Task> GetSubTasks(int taskId, List<Task> tasks)
        {
            var subTasks = tasks.Where(t => t.ParentId == taskId).ToList();
            foreach (var subTask in subTasks)
            {
                subTask.SubTasks = GetSubTasks(subTask.Id, tasks);
            }
            return subTasks;
        }
    }
}