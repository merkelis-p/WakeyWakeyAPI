using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T, R> : ControllerBase 
        where T : class 
        where R : IRepository<T>
    {
        private readonly R _repository;

        public GenericController(R repository)
        {
            _repository = repository;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/[controller]
        [HttpPost]
        public async Task<ActionResult<T>> Create(T entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction("GetById", new { id = (entity as dynamic).Id }, entity);
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, T entity)
        {
            if (id != (entity as dynamic).Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            _repository.Update(entity);
            return NoContent();
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}