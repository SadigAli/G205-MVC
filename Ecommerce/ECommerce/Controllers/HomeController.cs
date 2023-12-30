using AutoMapper;
using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public HomeController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Page = "Home";
            List<Product> products = await _productRepository.GetAllProductsWithDetails();
            List<ProductGetDTO> data = _mapper.Map<List<ProductGetDTO>>(products);
            return View(data);
        }

        public IActionResult Contact()
        {
            ViewBag.Page = "Contact";

            return View();
        }
    }
}
