using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(wakeyContext context, ILogger<CourseRepository> logger) : base(context, logger)
        {

        }
        
        // Get by user id
        public async Task<Course> GetByUserIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.UserId == id);
        }
        

    }
}