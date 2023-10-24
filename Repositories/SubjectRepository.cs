using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WakeyWakeyAPI.Repositories
{
    public class SubjectRepository : Repository<Subject>
    {
        public SubjectRepository(wakeyContext context, ILogger<SubjectRepository> logger) : base(context, logger)
        {
            
        }
        
        public async Task<IEnumerable<Subject>> GetSubjectsByCourse(int courseId)
        {
            return await _context.Subjects
                .Where(s => s.CourseId == courseId)
                .ToListAsync();
        }
        
    }
}