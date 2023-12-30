using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        public Task UpdateAsync(Color color,ColorPostDTO model);
    }
}
