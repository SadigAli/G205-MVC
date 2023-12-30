using AutoMapper;
using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.DTOs.SizeDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using System.Data;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        public ProductController(IProductRepository productRepository, IMapper mapper, IFileService fileService, IWebHostEnvironment env, IColorRepository colorRepository, ICategoryRepository categoryRepository,ISizeRepository sizeRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _fileService = fileService;
            _env = env;
            _colorRepository = colorRepository;
            _categoryRepository = categoryRepository;
            _sizeRepository = sizeRepository;
        }

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            List<Product> products = await _productRepository.GetAllProductsWithDetails();
            var data = _mapper.Map<List<ProductGetDTO>>(products);
            return View(data);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = _mapper.Map<List<CategoryGetDTO>>(await _categoryRepository.GetAllAsync());
            ViewBag.Colors = _mapper.Map<List<ColorGetDTO>>(await _colorRepository.GetAllAsync());
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductPostDTO model)
        {
            try
            {
                ViewBag.Categories = _mapper.Map<List<CategoryGetDTO>>(await _categoryRepository.GetAllAsync());
                ViewBag.Colors = _mapper.Map<List<ColorGetDTO>>(await _colorRepository.GetAllAsync());
                if (!ModelState.IsValid) return View(model);
                SlugHelper helper = new SlugHelper();
                (bool status, string message) = await _fileService.FileUpload(model.PosterFile, _env.WebRootPath, "products");
                if (!status) ModelState.AddModelError("PosterFile", message);
                if (!ModelState.IsValid) return View(model);
                Product product = _mapper.Map<Product>(model);
                product.ProductImages = new List<ProductImage>();
                product.ProductImages.Add(new ProductImage
                {
                    Status = ImageStatus.Poster,
                    Image = message,
                });
                product.Slug = helper.GenerateSlug(model.Name);

                await _productRepository.AddImages(product, model.Files,_env.WebRootPath);
                product.ProductColors = new List<ProductColor>();
                _productRepository.AddColors(product, model.ColorIds);
                await _productRepository.CreateAsync(product);
                TempData["Message"] = "Product has been successfully added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Product product = await _productRepository.GetProductWithDetails(id);
            var data = _mapper.Map<ProductPutDTO>(product);
            ViewBag.Categories = _mapper.Map<List<CategoryGetDTO>>(await _categoryRepository.GetAllAsync());
            ViewBag.Colors = _mapper.Map<List<ColorGetDTO>>(await _colorRepository.GetAllAsync());
            return View(data);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductPutDTO model)
        {
            try
            {
                ViewBag.Categories = _mapper.Map<List<CategoryGetDTO>>(await _categoryRepository.GetAllAsync());
                ViewBag.Colors = _mapper.Map<List<ColorGetDTO>>(await _colorRepository.GetAllAsync());
                if (!ModelState.IsValid) return View(model);
                Product product = await _productRepository.GetProductWithDetails(id);
                if(model.PosterFile != null)
                {
                    (bool status, string result) = await _fileService.FileUpload(model.PosterFile, _env.WebRootPath, "products");
                    if (!status) ModelState.AddModelError("PosterFile", result);
                    if (!ModelState.IsValid) return View(model);
                    _fileService.FileDelete(_env.WebRootPath, "products", product.ProductImages.FirstOrDefault(x => x.Status == ImageStatus.Poster).Image);
                    _productRepository.RemoveImages(product, ImageStatus.Poster);
                    product.ProductImages.Add(new ProductImage
                    {
                        Image = result,
                        Status = ImageStatus.Poster
                    });
                }

                if(model.Files != null)
                {
                    _productRepository.RemoveImages(product, ImageStatus.Normal);
                    await _productRepository.AddImages(product, model.Files, _env.WebRootPath);
                }
                await _productRepository.Update(product, model);
                TempData["Message"] = "Product has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> EditProductColor(int id)
        {
            ProductColor productColor = await _productRepository.GetProductColor(id);
            var data = _mapper.Map<ProductColorPostDTO>(productColor);
            ViewBag.Sizes = _mapper.Map<List<SizeGetDTO>>(await _sizeRepository.GetAllAsync());
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductColor(int id, ProductColorPostDTO model)
        {
            ProductColor productColor = await _productRepository.GetProductColor(id);
            await _productRepository.AddSizes(productColor, model.SizeIds);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditSize(int id)
        {
            ProductColorSize productColorSize = await _productRepository.GetProductColorSize(id);
            ProductColorSizeDTO data = _mapper.Map<ProductColorSizeDTO>(productColorSize);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSize(int id, ProductColorSizeDTO model)
        {
            ProductColorSize productColorSize = await _productRepository.GetProductColorSize(id);
            await _productRepository.UpdateProductColorSize(productColorSize, model);
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Product product = await _productRepository.GetProductWithDetails(id);
                foreach (ProductImage image in product.ProductImages)
                {
                    _fileService.FileDelete(_env.WebRootPath, "products", image.Image);
                }
                _productRepository.Delete(product);
                return Json(new
                {
                    Message = "Product has been deleted successfully",
                    Status = true
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = ex.Message,
                    Status = false
                });
            }
        }
    }
}
