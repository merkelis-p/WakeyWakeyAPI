using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;


namespace WakeyWakeyAPI.Controllers
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly wakeyContext _context;
        private readonly DbSet<T> _entities;

        public Repository(wakeyContext context)
        {
            _context = context;
            _entities = context.Set<T>();
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
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
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