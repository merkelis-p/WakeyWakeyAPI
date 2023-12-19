using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;

public interface ISubjectController : IGenericController<Subject>
{
    Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByCourse(int courseId);
}