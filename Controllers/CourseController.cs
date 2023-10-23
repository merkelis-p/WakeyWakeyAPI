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

    }
}