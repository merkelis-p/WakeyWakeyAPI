using System.Collections.Generic;
using System.Threading.Tasks;
using WakeyWakeyAPI.Models;
using Task = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<List<Course>> GetByUserIdAsync(int id);

        public Task<List<Course>> GetAllHierarchyAsync(int userId);

        public Task<ICollection<Task>> GetSubTasksAsync(int taskId);
    }
}