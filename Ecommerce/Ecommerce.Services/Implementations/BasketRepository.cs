using Ecommerce.Data.DTOs.BasketDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ApplicationContext _context;
        public BasketRepository(IHttpContextAccessor accessor, ApplicationContext context)
        {
            _accessor = accessor;
            _context = context;
        }

        public async Task<(bool,string)> AddToCart(int id, int count=1)
        {
            ProductColorSize productColorSize = await _context.ProductColorSizes
                                        .Include(x => x.ProductColor)
                                        .ThenInclude(x => x.Color)
                                        .Include(x => x.ProductColor)
                                        .ThenInclude(x => x.Product)
                                        .ThenInclude(x => x.ProductImages)
                                        .Include(x => x.Size)
                                        .FirstOrDefaultAsync(x=>x.Id == id);
            if(productColorSize == null) return (false, "This product doesn't exists");
            if (productColorSize.Count < count) return (false, "There is no enough product in stock");
            List<BasketGetDTO> basketList = new List<BasketGetDTO>();
            string basket = _accessor.HttpContext.Request.Cookies["basket"];
            if(string.IsNullOrEmpty(basket))
            {
                basketList.Add(new BasketGetDTO
                {
                    ProductId = productColorSize.Id,
                    Image = productColorSize.ProductColor.Product.ProductImages.FirstOrDefault(x=>x.Status == ImageStatus.Poster).Image,
                    ColorName = productColorSize.ProductColor.Color.Name,
                    ProductName = productColorSize.ProductColor.Product.Name,
                    SizeName = productColorSize.Size.Name,
                    Count = count,
                    ProductPrice = productColorSize.ProductColor.Product.Price(),
                });

            }
            else
            {
                basketList = JsonConvert.DeserializeObject<List<BasketGetDTO>>(basket);
                BasketGetDTO inBasket = basketList.FirstOrDefault(b => b.ProductId == productColorSize.Id);
                if(inBasket != null)
                {
                    inBasket.Count += count;
                }
                else
                {
                    basketList.Add(new BasketGetDTO
                    {
                        ProductId = productColorSize.Id,
                        Image = productColorSize.ProductColor.Product.ProductImages.FirstOrDefault(x => x.Status == ImageStatus.Poster).Image,
                        ColorName = productColorSize.ProductColor.Color.Name,
                        ProductName = productColorSize.ProductColor.Product.Name,
                        SizeName = productColorSize.Size.Name,
                        Count = count,
                        ProductPrice = productColorSize.ProductColor.Product.Price(),
                    });
                }
            }
            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketList));

            return (true, "Product has been added successfully to basket");
        }

        public void ClearBasket()
        {
            _accessor.HttpContext.Response.Cookies.Delete("basket");
        }

        public List<BasketGetDTO> GetAll()
        {
            List<BasketGetDTO> basketList = new List<BasketGetDTO>();
            string basket = _accessor.HttpContext.Request.Cookies["basket"];
            if(basket != null)
            {
                basketList = JsonConvert.DeserializeObject<List<BasketGetDTO>>(basket);
            }
            return basketList;
        }

        public (bool,string) RemoveFromCart(int id)
        {
            string basket = _accessor.HttpContext.Request.Cookies["basket"];
            List<BasketGetDTO> basketList = JsonConvert.DeserializeObject<List<BasketGetDTO>>(basket);
            BasketGetDTO inBasket = basketList.FirstOrDefault(x => x.ProductId == id);
            if (inBasket is null) return (false, "No product in basket");
            basketList.Remove(inBasket);
            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketList));
            return (true, "Product has been deleted from basket");
        }
    }
}
