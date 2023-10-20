using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class SubjectRepository : Repository<Subject>
    {
        public SubjectRepository(wakeyContext context) : base(context)
        {
            
        }

    }
}