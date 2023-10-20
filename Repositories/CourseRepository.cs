using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(wakeyContext context) : base(context)
        {
            
        }

    }
}