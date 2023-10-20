using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class SubjectController : GenericController<Subject, SubjectRepository>
    {
        
        SubjectRepository _context;
        public SubjectController(SubjectRepository context) : base(context)
        {
            _context = context;

        }
        
    }
}