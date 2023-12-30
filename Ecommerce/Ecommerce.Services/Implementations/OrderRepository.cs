using Ecommerce.Data.DTOs.BasketDTO;
using Ecommerce.Data.DTOs.OrderDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LayoutService _layout;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationContext _context;
        public OrderRepository(LayoutService layout, IHttpContextAccessor accessor,UserManager<AppUser> usermanager, IBasketRepository basketRepository, ApplicationContext context, IProductRepository productRepository)
        {
            _layout = layout;
            _accessor = accessor;
            _usermanager = usermanager;
            _basketRepository = basketRepository;
            _context = context;
            _productRepository = productRepository;
        }

        public async Task ChangeStatus(int id, int status)
        {
            Order order = await GetOrder(id);
            order.OrderStatus = (OrderStatus)status;
            if(status == 1)
            {
                foreach (OrderProduct orderProduct in order.OrderProducts)
                {
                    ProductColorSize productColorSize = await _productRepository.GetProductColorSize(orderProduct.ProductColorSizeId);
                    productColorSize.Count -= orderProduct.Count;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(x=>x.AppUser)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.ProductColorSize)
                .ThenInclude(x => x.Size)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.ProductColorSize)
                .ThenInclude(x => x.ProductColor)
                .ThenInclude(x => x.Color)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.ProductColorSize)
                .ThenInclude(x => x.ProductColor)
                .ThenInclude(x => x.Product)
                .Where(x => x.DeletedAt == null).ToListAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                            .Include(x => x.OrderProducts)
                            .ThenInclude(x => x.ProductColorSize)
                            .ThenInclude(x => x.Size)
                            .Include(x => x.OrderProducts)
                            .ThenInclude(x => x.ProductColorSize)
                            .ThenInclude(x => x.ProductColor)
                            .ThenInclude(x => x.Color)
                            .Include(x => x.OrderProducts)
                            .ThenInclude(x => x.ProductColorSize)
                            .ThenInclude(x => x.ProductColor)
                            .ThenInclude(x => x.Product)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task ProductOrder(OrderGetDTO model)
        {
            List<BasketGetDTO> baskets = _layout.GetBasket();
            AppUser user = await _usermanager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);
            Order order = new Order()
            {
                ZipCode = model.ZipCode,
                Address = model.Address,
                AppUserId = user.Id,
                Phone = model.Phone,
                TotalPrice = baskets.Sum(x=>x.Count * x.ProductPrice) + 10
            };

            order.OrderProducts = new List<OrderProduct>();
            foreach (BasketGetDTO item in baskets)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    Count = item.Count,
                    ProductColorSizeId = item.ProductId,
                    ProductPrice = item.ProductPrice,
                });
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _basketRepository.ClearBasket();
        }
    }
}
