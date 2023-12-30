using AutoMapper;
using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository, IMapper mapper, ISizeRepository sizeRepository, IColorRepository colorRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Page = "Shop";
            List<Product> products = await _productRepository.GetAllProductsWithDetails();
            List<ProductGetDTO> data = _mapper.Map<List<ProductGetDTO>>(products);
            ViewBag.Sizes = await _sizeRepository.GetAllAsync();
            ViewBag.Colors = await _colorRepository.GetAllAsync();
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Detail(string slug)
        {
            ViewBag.Page = "Shop";
            Product product = await _productRepository.GetProductBySlug(slug);
            ProductGetDTO data = _mapper.Map<ProductGetDTO>(product);   
            return View(data);
        }

        // product-detail?id=5

        public async Task<IActionResult> GetProductSizes(int id)
        {
            ProductColor productColor = await _productRepository.GetProductColor(id);
            List<ProductColorSizeDTO> sizes = _mapper.Map<List<ProductColorSizeDTO>>(productColor.ProductColorSizes);
            return PartialView("_SizePartial",sizes);
        }

        public IActionResult Filter(string? search= null,
            string? size = null,
            string? category = null,
            string? color = null,
            int? min = null,
            int? max = null)
        {
            List<Product> products = _productRepository.Filter(search,color,size,category,min,max);
            var data = _mapper.Map<List<ProductGetDTO>>(products);
            return PartialView("_ProductPartial",data);
        }
    }
}
