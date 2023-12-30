using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task CreateAsync(T entity);
        public void Delete(T entity);
        public bool IsExists(int id);
    }
}
