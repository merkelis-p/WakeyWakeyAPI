using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class RecordController : GenericController<Record, RecordRepository>, IRecordController
    {
        
        RecordRepository _context;
        public RecordController(RecordRepository context) : base(context)
        {
            
            _context = context;
            
        }
        
    }

}