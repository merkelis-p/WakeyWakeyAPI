using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Repositories;
using Task = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Controllers
{
    public class TaskController: GenericController<Task, TaskRepository>
    {
        
        TaskRepository _context;
        public TaskController(TaskRepository context) : base(context)
        {
            _context = context;
        }
        
        // Get by user id
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Task>> GetByUserId(int id)
        {
            
            var task = await _context.GetByUserIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return task;
            
        }
        
        // Get by subject id
        [HttpGet("GetBySubjectId/{id}")]
        public async Task<ActionResult<Task>> GetBySubjectId(int id)
        {

            var task = await _context.GetBySubjectIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return task;

        }
        
        // Get children by parent id
        [HttpGet("GetChildrenByParentId/{id}")]
        public async Task<ActionResult<Task>> GetChildrenByParentId(int id)
        {

            var task = await _context.GetChildrenByParentIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return task;

        }
        
        // Get tasks with hierarchy and where user id is equal to the given id
        [HttpGet("GetTasksWithHierarchyByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksWithHierarchyByUserId(int id)
        {

            var tasks = await _context.GetTasksWithHierarchyByUserId(id);
            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);

        }
        
        // Get tasks with hierarchy
        [HttpGet("GetTasksWithHierarchy")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksWithHierarchy()
        {

            var tasks = await _context.GetTasksWithHierarchy();
            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);

        }
        


    }
}