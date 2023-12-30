using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.DTOs.SizeDTO;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface ISizeRepository : IGenericRepository<Size>
    {
        public Task UpdateAsync(Size size, SizePostDTO model);

    }
}
