using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Repositories
{
    public class RecordRepository : Repository<Record>
    {
        public RecordRepository(wakeyContext context) : base(context)
        {
            
        }

    }
}