using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Repositories
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        public RecordRepository(wakeyContext context, ILogger<RecordRepository> logger) : base(context, logger)
        {
            
        }

    }
}