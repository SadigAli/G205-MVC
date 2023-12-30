using Ecommerce.Data.DTOs.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IBasketRepository
    {
        public void ClearBasket();
        public List<BasketGetDTO> GetAll();
        public Task<(bool,string)> AddToCart(int productId,int count=1);
        public (bool,string) RemoveFromCart(int id);
    }
}
