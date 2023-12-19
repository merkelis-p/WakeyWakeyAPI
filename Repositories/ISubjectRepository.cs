using System.Collections.Generic;
using System.Threading.Tasks;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        public Task<IEnumerable<Subject>> GetSubjectsByCourse(int courseId);

    }
}