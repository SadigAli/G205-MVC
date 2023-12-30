using Ecommerce.Data.DTOs.OrderDTO;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IOrderRepository
    {
        public Task ProductOrder(OrderGetDTO model);
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrder(int id);
        public Task ChangeStatus(int id,int status);
    }
}
