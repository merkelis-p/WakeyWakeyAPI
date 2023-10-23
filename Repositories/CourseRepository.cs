using Microsoft.Extensions.Logging;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(wakeyContext context, ILogger<CourseRepository> logger) : base(context, logger)
        {

        }
        

    }
}