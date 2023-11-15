using System.Collections.Generic;
using System.Linq;
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
        
        // Get tasks with hierarchy and where subject id is equal to null
        public async Task<IEnumerable<Task>> GetIndependentTasksWithHierarchy(int id)
        {
            var allTasks = await _context.Tasks.ToListAsync();
            return allTasks.Where(task => task.ParentId == null && task.SubjectId == id)
                .Select(task => new Task 
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    SubjectId = task.SubjectId,
                    Category = task.Category,
                    UserId = task.UserId,
                    EstimatedDuration = task.EstimatedDuration,
                    OverallDuration = task.OverallDuration,
                    DeadlineDate = task.DeadlineDate,
                    Score = task.Score,
                    ScoreWeight = task.ScoreWeight,
                    Status = task.Status,
                    SubTasks = GetSubTasks(task.Id, allTasks)
                });
        }
        
        
        // Get tasks with hierarchy and where subject id is equal to the given id
        public async Task<IEnumerable<Task>> GetTasksWithHierarchyBySubjectId(int id)
        {
            var allTasks = await _context.Tasks.ToListAsync();
            return allTasks.Where(task => task.ParentId == null && task.SubjectId == id)
                .Select(task => new Task 
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    SubjectId = task.SubjectId,
                    Category = task.Category,
                    UserId = task.UserId,
                    EstimatedDuration = task.EstimatedDuration,
                    OverallDuration = task.OverallDuration,
                    DeadlineDate = task.DeadlineDate,
                    Score = task.Score,
                    ScoreWeight = task.ScoreWeight,
                    Status = task.Status,
                    SubTasks = GetSubTasks(task.Id, allTasks)
                });
        }
        
        
        
        // Get tasks with hierarchy and where user id is equal to the given id
        public async Task<IEnumerable<Task>> GetTasksWithHierarchyByUserId(int id)
        {
            var allTasks = await _context.Tasks.ToListAsync();
            return allTasks.Where(task => task.ParentId == null && task.UserId == id)
                .Select(task => new Task 
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    SubjectId = task.SubjectId,
                    Category = task.Category,
                    UserId = task.UserId,
                    EstimatedDuration = task.EstimatedDuration,
                    OverallDuration = task.OverallDuration,
                    DeadlineDate = task.DeadlineDate,
                    Score = task.Score,
                    ScoreWeight = task.ScoreWeight,
                    Status = task.Status,
                    SubTasks = GetSubTasks(task.Id, allTasks)
                });
        }
        
        public async Task<IEnumerable<Task>> GetTasksWithHierarchy()
        {
            var allTasks = await _context.Tasks.ToListAsync();
            return allTasks.Where(task => task.ParentId == null)
                .Select(task => new Task 
                {
                   Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    SubjectId = task.SubjectId,
                    Category = task.Category,
                    UserId = task.UserId,
                    EstimatedDuration = task.EstimatedDuration,
                    OverallDuration = task.OverallDuration,
                    DeadlineDate = task.DeadlineDate,
                    Score = task.Score,
                    ScoreWeight = task.ScoreWeight,
                    Status = task.Status,
                    SubTasks = GetSubTasks(task.Id, allTasks)
                });
        }

        private List<Task> GetSubTasks(int parentId, IEnumerable<Task> allTasks)
        {
            var subTasks = allTasks.Where(task => task.ParentId == parentId).ToList();
            foreach (var subTask in subTasks)
            {
                subTask.SubTasks = GetSubTasks(subTask.Id, allTasks);
            }
            return subTasks;
        }
        
        
        
        
        
        

        
    }
}