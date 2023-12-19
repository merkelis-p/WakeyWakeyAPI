using System.Collections.Generic;
using System.Threading.Tasks;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        public  Task<IEnumerable<Event>> GetByUserIdAsync(int id);

    }
}