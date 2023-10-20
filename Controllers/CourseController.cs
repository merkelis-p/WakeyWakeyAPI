using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class CourseController : GenericController<Course, CourseRepository>
    {
        CourseRepository _context;
        public CourseController(CourseRepository context) : base(context)
        {
            _context = context;
        }

    }
}