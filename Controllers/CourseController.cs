using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class CourseController : GenericController<Course, CourseRepository>
    {
        public CourseController(CourseRepository context) : base(context)
        {

        }

    }
}