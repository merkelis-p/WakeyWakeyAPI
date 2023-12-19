using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Controllers
{
    public interface ICourseController : IGenericController<Course>
    {
        Task<ActionResult<List<Course>>> GetByUserId(int id);
        Task<ActionResult<List<Course>>> GetAllHierarchy(int id);
    }

}