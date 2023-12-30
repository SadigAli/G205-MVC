using Ecommerce.Data.DTOs.OrderDTO;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public CheckoutController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Page = "Checkout";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order(OrderGetDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please, fill all inputs");
                return View("Index");
            }
            await _orderRepository.ProductOrder(model);
            TempData["Message"] = "Order has been completed successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
