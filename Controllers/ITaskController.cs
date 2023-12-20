using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;
using Task = WakeyWakeyAPI.Models.Task;

public interface ITaskController : IGenericController<Task>
{
    Task<ActionResult<Task>> GetBySubjectId(int id);
    Task<ActionResult<Task>> GetChildrenByParentId(int id);
    Task<ActionResult<IEnumerable<Task>>> GetTasksWithHierarchyByUserId(int id);
    Task<ActionResult<IEnumerable<Task>>> GetTasksWithHierarchy();
    Task<ActionResult<Task>> CreateTask(TaskCreateRequest taskCreateRequest);
}