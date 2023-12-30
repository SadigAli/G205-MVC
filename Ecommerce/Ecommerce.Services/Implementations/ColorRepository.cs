using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task UpdateAsync(Color color, ColorPostDTO model)
        {
            color.UpdatedAt = DateTime.Now;
            color.Name = model.Name;
            await _context.SaveChangesAsync(); 
        }
    }
}
