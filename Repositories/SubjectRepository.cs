using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class SubjectRepository : Repository<Subject>
    {
        public SubjectRepository(wakeyContext context, ILogger<SubjectRepository> logger) : base(context, logger)
        {
            
        }

    }
}