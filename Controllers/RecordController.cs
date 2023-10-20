using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class RecordController : GenericController<Record, RecordRepository>
    {
        public RecordController(RecordRepository context) : base(context)
        {
            
        }
        
    }

}