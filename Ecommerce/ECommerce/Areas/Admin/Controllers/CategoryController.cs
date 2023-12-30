using AutoMapper;
using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slugify;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public CategoryController(IFileService fileService, ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _env = env;
        }

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            List<Category> categories = await  _categoryRepository.GetAllAsync();
            var data = _mapper.Map<List<CategoryGetDTO>>(categories);
            return View(data);
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if(category is null) return NotFound();
            var data = _mapper.Map<CategoryGetDTO>(category);
            return View(data);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryPostDTO model)
        {
            try
            {
                SlugHelper helper = new SlugHelper();
                if (!ModelState.IsValid) return View(model);
                (bool status,string message) = await _fileService.FileUpload(model.File, _env.WebRootPath, "categories");
                if (!status) ModelState.AddModelError("File", message);
                if(!ModelState.IsValid) return View(model);
                
                Category category = _mapper.Map<Category>(model);
                category.Slug = helper.GenerateSlug(model.Name);
                category.Image = message;
                await _categoryRepository.CreateAsync(category);
                TempData["Message"] = "Category has been successfully added";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return NotFound();
            var data = _mapper.Map<CategoryPutDTO>(category);
            return View(data);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CategoryPutDTO model)
        {
            try
            {
                SlugHelper helper = new SlugHelper();
                Category category = await _categoryRepository.GetByIdAsync(id);
                if (category is null) return NotFound();
                model.Image = category.Image;
                if(model.File != null)
                {
                    (bool status, string message) = await _fileService.FileUpload(model.File, _env.WebRootPath, "categories");
                    if (!status) ModelState.AddModelError("File", message);
                    if (!ModelState.IsValid) return View(model);
                    _fileService.FileDelete(_env.WebRootPath, "categories", category.Image);
                    model.Image = message;
                }
                model.Slug = helper.GenerateSlug(model.Name);

                await _categoryRepository.UpdateAsync(category,model);
                TempData["Message"] = "Category has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Category category = await _categoryRepository.GetByIdAsync(id);
                if (category is null) return NotFound();
                _fileService.FileDelete(_env.WebRootPath, "categories", category.Image);
                _categoryRepository.Delete(category);
                return Json(new
                {
                    Message = "Category has been deleted successfully",
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
