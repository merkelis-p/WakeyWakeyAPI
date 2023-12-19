using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace WakeyWakeyAPI.Controllers
{
    
    public class CourseController : GenericController<Course, CourseRepository>, ICourseController
    {
        CourseRepository _context;
        public CourseController(CourseRepository context) : base(context)
        {
            _context = context;
        }
        
        // Get by user id
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<List<Course>>> GetByUserId(int id)
        {
            var courses = await _context.GetByUserIdAsync(id);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }

            return Ok(courses);
        }
        
        // get all hierarchy
        [HttpGet("GetAllHierarchy/{id}")]
        public async Task<ActionResult<List<Course>>> GetAllHierarchy(int id)
        {
            var courses = await _context.GetAllHierarchyAsync(id);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }

            return Ok(courses);
        }
    }

}