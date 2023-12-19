using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using WakeyWakeyAPI.Models;
using TaskM = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Repositories
{
    public interface ITaskRepository : IRepository<TaskM>
    {

         
        // Get by user id
        public Task<IEnumerable<TaskM>> GetByUserIdAsync(int id);
        
        // Get by subject id
        public  Task<IEnumerable<TaskM>> GetBySubjectIdAsync(int id);
        
        // Get children by parent id
        public  Task<IEnumerable<TaskM>> GetChildrenByParentIdAsync(int id);
        // Get tasks with hierarchy and where subject id is equal to null
        public Task<IEnumerable<TaskM>> GetIndependentTasksWithHierarchy(int id);


        public Task<IEnumerable<TaskM>> GetTasksWithHierarchyBySubjectId(int id);
        
        
        // Get tasks with hierarchy and where user id is equal to the given id
        public Task<IEnumerable<TaskM>> GetTasksWithHierarchyByUserId(int id);

        public Task<IEnumerable<TaskM>> GetTasksWithHierarchy();
        public  IEnumerable<TaskM> GetSubTasks(int parentId, IEnumerable<TaskM> allTasks);


        public Task<TaskM> AddTaskAsync(TaskCreateRequest taskCreateRequest);



    }
}