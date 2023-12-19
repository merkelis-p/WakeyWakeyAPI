using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Task = WakeyWakeyAPI.Models.Task;

namespace WakeyWakeyAPI.Controllers
{
    public class TaskController: GenericController<Task, ITaskRepository>, ITaskController
    {
        
        ITaskRepository _context;
        public TaskController(ITaskRepository context) : base(context)
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

            return Ok(task);
            
        }
        
        // Get by subject id
        [HttpGet("GetBySubjectId/{id}")]
        public async Task<ActionResult<Task>> GetBySubjectId(int id)
        {

            var task = await _context.GetTasksWithHierarchyBySubjectId(id);
            if (task == null || !task.Any())
            {
                return NotFound();
            }

            return Ok(task);

        }
        
        // Get children by parent id
        [HttpGet("GetChildrenByParentId/{id}")]
        public async Task<ActionResult<Task>> GetChildrenByParentId(int id)
        {

            var task = await _context.GetChildrenByParentIdAsync(id);
            if (task == null || !task.Any())
            {
                return NotFound();
            }

            return Ok(task);

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
        
        
        [HttpPost("CreateTask")]
        public async Task<ActionResult<Task>> CreateTask(TaskCreateRequest taskCreateRequest)
        {
            var task = await _context.AddTaskAsync(taskCreateRequest);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

    }
}