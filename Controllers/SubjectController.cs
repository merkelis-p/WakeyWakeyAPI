using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class SubjectController : GenericController<Subject, SubjectRepository>
    {
        public SubjectController(SubjectRepository context) : base(context)
        {
            
        }
        
    }
}