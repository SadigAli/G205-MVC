using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task UpdateAsync(Category category, CategoryPutDTO model)
        {
            category.Image = model.Image;
            category.Name = model.Name;
            category.Slug = model.Slug;
            category.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
