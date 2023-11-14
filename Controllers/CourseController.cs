using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WakeyWakeyAPI.Controllers
{
    public class CourseController : GenericController<Course, CourseRepository>
    {
        CourseRepository _context;
        public CourseController(CourseRepository context) : base(context)
        {
            _context = context;
        }
        
        // Get by user id
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Course>> GetByUserId(int id)
        {
            var course = await _context.GetByUserIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

    }
}