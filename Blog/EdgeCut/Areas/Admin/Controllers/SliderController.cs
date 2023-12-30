using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SliderController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;
        public SliderController(ApplicationContext context,IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        // GET: SliderController
        public ActionResult Index()
        {
            List<Slider> sliders = _context.Sliders
                                        .Where(x=>x.DeletedAt == null)
                                        .ToList();
            return View(sliders);
        }

        // GET: SliderController/Details/5
        public ActionResult Details(int id)
        {
            Slider slider = _context.Sliders
                 .Where(x=>x.DeletedAt ==null)
                .FirstOrDefault(s => s.Id == id);
            if(slider is null) return NotFound();
            return View(slider);
        }

        // GET: SliderController/Create
        public ActionResult Create() // admin/slider/create
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Slider slider) // admin/slider/create
        {
            try
            {
                if (!ModelState.IsValid) return View(slider);
                (int status, string message) = await _fileService.FileUpload("sliders",slider.File);
                if (status == 0) ModelState.AddModelError("File", message);
                if (!ModelState.IsValid) return View(slider);
                slider.Image = message;
                _context.Sliders.Add(slider);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Slider has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SliderController/Edit/5
        public ActionResult Edit(int id)
        {
            Slider slider = _context.Sliders
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x=>x.Id ==id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Slider model)
        {
            try
            {
                Slider slider = _context.Sliders
                    .Where(x => x.DeletedAt == null)
                    .FirstOrDefault(x => x.Id == id);
                if (slider is null) return NotFound();
                if(model.File  != null)
                {
                    (int status, string message) = await _fileService.FileUpload("sliders", model.File);
                    if(status == 0)
                    {
                        ModelState.AddModelError("File", message);
                        return View(slider);
                    }
                    _fileService.DeleteFile("sliders", slider.Image);
                    slider.Image = message;
                }
                slider.Title = model.Title;
                slider.Description = model.Description;
                slider.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Slider has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SliderController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
                if (slider is null) return NotFound();
                slider.DeletedAt = DateTime.Now;
                _fileService.DeleteFile("sliders", slider.Image);
                _context.SaveChanges();
                return Json(new
                {
                    Status = true,
                    Message = "Slider has been deleted",
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
