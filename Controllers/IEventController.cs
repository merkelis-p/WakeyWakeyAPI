using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;

namespace WakeyWakeyAPI.Controllers
{
    public interface IEventController : IGenericController<Event>
    {
        Task<ActionResult<Event>> GetByUserId(int id);
    }

}