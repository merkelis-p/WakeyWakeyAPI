using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;



namespace WakeyWakeyAPI.Controllers
{
    public class SubjectController : GenericController<Subject, SubjectRepository>, ISubjectController
    {
        
        SubjectRepository _context;
        public SubjectController(SubjectRepository context) : base(context)
        {
            _context = context;

        }
        
        [HttpGet("GetSubjectsByCourse/{courseId}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByCourse(int courseId)
        {
            var subjects = await _context.GetSubjectsByCourse(courseId);
            if (subjects == null || !subjects.Any())
            {
                return NotFound();
            }
            return Ok(subjects);
        }



    }
}