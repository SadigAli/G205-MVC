using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.DTOs.SizeDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task UpdateAsync(Size size, SizePostDTO model)
        {

            size.UpdatedAt = DateTime.Now;
            size.Name = model.Name;
            await _context.SaveChangesAsync();
        }
    }
}
