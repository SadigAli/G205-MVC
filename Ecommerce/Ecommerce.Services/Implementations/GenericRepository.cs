using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _table;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            entity.DeletedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _table.Where(x => x.DeletedAt == null).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _table.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool IsExists(int id)
        {
            return _table.Where(x => x.DeletedAt == null).Any(x => x.Id == id);
        }
    }
}
