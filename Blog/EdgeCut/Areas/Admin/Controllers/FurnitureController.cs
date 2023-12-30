using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class FurnitureController : Controller
    {
        // GET: FurnitureController
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;

        public FurnitureController(ApplicationContext context,IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public ActionResult Index()
        {
            List<Furniture> furnitures = _context.Furnitures
                                    .Where(x=>x.DeletedAt == null)
                                    .ToList();
            return View(furnitures);
        }

        // GET: FurnitureController/Details/5
        public ActionResult Details(int id)
        {
            Furniture furniture = _context.Furnitures
                                .Where(x => x.DeletedAt == null)
                                .FirstOrDefault(x => x.Id == id);
            if (furniture is null) return NotFound();
            return View(furniture);
        }

        // GET: FurnitureController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FurnitureController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Furniture furniture)
        {
            try
            {
                if(furniture.File is null)
                    ModelState.AddModelError("File", "File is required");
                if (!ModelState.IsValid) return View(furniture);
                (int status, string message) = await _fileService.FileUpload("furnitures", furniture.File);
                if(status == 0)
                {
                    ModelState.AddModelError("File", message);
                    return View(furniture);
                }
                furniture.Image = message;
                _context.Furnitures.Add(furniture);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Furniture has been added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: FurnitureController/Edit/5
        public ActionResult Edit(int id)
        {
            Furniture furniture = _context.Furnitures
                                .Where(x => x.DeletedAt == null)
                                .FirstOrDefault(x => x.Id == id);
            if (furniture is null) return NotFound();

            return View(furniture);
        }

        // POST: FurnitureController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Furniture model)
        {
            try
            {
                Furniture furniture = _context.Furnitures
                                .Where(x => x.DeletedAt == null)
                                .FirstOrDefault(x => x.Id == id);
                if (furniture is null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("furnitures", model.File);
                    if (status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(model);
                    }
                    _fileService.DeleteFile("furnitures", furniture.Image);
                    furniture.Image = message;
                }
                furniture.Name = model.Name;
                furniture.Price = model.Price;
                furniture.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Furniture has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: FurnitureController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Furniture furniture = _context.Furnitures.FirstOrDefault(x => x.Id == id);
                if (furniture is null) return NotFound();
                furniture.DeletedAt = DateTime.Now;
                _fileService.DeleteFile("furnitures", furniture.Image);
                _context.SaveChanges();
                return Json(new
                {
                    Status = true,
                    Message = "Furniture has been deleted",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = "Something went wrong"
                });
            }
        }
    }
}
