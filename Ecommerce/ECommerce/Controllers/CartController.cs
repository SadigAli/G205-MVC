using Ecommerce.Data.DTOs.BasketDTO;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly IBasketRepository _basketRepository;
        public CartController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public IActionResult Index()
        {
            List<BasketGetDTO> basketList = _basketRepository.GetAll();
            ViewBag.Page = "Cart";
            return View(basketList);
        }
        [HttpGet("/add-to-cart/{sizeId}")]
        public  async Task<IActionResult> AddToCart(int sizeId)
        {
            (bool status,string message) = await _basketRepository.AddToCart(sizeId);
            return Content(message);
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            (bool status,string message) = _basketRepository.RemoveFromCart(id);
            return Content(message);
        }

    }
}
