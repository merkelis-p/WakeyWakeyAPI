using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace WakeyWakeyAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly wakeyContext _context;
        private readonly DbSet<T> _entities;

        private readonly ILogger<Repository<T>> _logger;

        public Repository(wakeyContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _entities = context.Set<T>();
            _logger = logger;
        }


        public async System.Threading.Tasks.Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async System.Threading.Tasks.Task AddAsync(T entity)
        {
            _logger.LogInformation($"Starting creation of a new {typeof(T).Name} entity.");
            _logger.LogInformation($"Entity Data: {JsonSerializer.Serialize(entity)}");
            
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"{typeof(T).Name} entity successfully added.");
        }

        public void Update(T entity)
        {
            _logger.LogInformation($"Starting update for the {typeof(T).Name} entity.");
            _logger.LogInformation($"Entity Data: {JsonSerializer.Serialize(entity)}");

            _entities.Update(entity);
            _context.SaveChanges();

            _logger.LogInformation($"{typeof(T).Name} entity successfully updated.");
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            T entity = await _entities.FindAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _entities.FindAsync(id) != null;
        }
    }

}