using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class RecordController : GenericController<Record, IRecordRepository>, IRecordController
    {
        
        IRecordRepository _context;
        public RecordController(IRecordRepository context) : base(context)
        {
            
            _context = context;
            
        }
        
    }

}