using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task UpdateAsync(Category category,CategoryPutDTO model);
    }
}
