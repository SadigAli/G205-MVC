using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class TestimonialController : Controller
    {
        // GET: TestimonialController
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;
        public TestimonialController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        // GET: TestimonialController
        public ActionResult Index()
        {
            List<Testimonial> testimonials = _context.Testimonials.Where(x => x.DeletedAt == null).ToList();
            return View(testimonials);
        }

        // GET: TestimonialController/Details/5
        public ActionResult Details(int id)
        {
            Testimonial testimonial = _context.Testimonials
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
            if (testimonial is null) return NotFound();
            return View(testimonial);
        }

        // GET: TestimonialController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestimonialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Testimonial testimonial)
        {
            try
            {
                if (testimonial.File is null) ModelState.AddModelError("File", "File is required");
                (int status, string message) = await _fileService.FileUpload("testimonials", testimonial.File);
                if (status == 0) ModelState.AddModelError("File", message);
                if (!ModelState.IsValid) return View(testimonial);
                testimonial.Image = message;
                _context.Testimonials.Add(testimonial);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Testimonial has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: TestimonialController/Edit/5
        public ActionResult Edit(int id)
        {
            Testimonial testimonial = _context.Testimonials
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
            if (testimonial is null) return NotFound();

            return View(testimonial);
        }

        // POST: TestimonialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Testimonial model)
        {
            try
            {
                Testimonial testimonial = _context.Testimonials
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
                if (testimonial is null) return NotFound();
                if (model.File != null)
                {
                    (int status, string message) = await _fileService.FileUpload("testimonials", model.File);
                    if (status == 0) ModelState.AddModelError("File", message);
                    if (!ModelState.IsValid) return View(model);
                    _fileService.DeleteFile("testimonials", testimonial.Image);
                    testimonial.Image = message;
                }
                testimonial.Name = model.Name;
                testimonial.Description = model.Description;
                testimonial.UpdatedAt = DateTime.Now;
                _context.SaveChangesAsync();
                TempData["Message"] = "Testimonial has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: TestimonialController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Testimonial testimonial = _context.Testimonials
                            .Where(x => x.DeletedAt == null)
                            .FirstOrDefault(x => x.Id == id);
                if (testimonial is null) return NotFound();
                testimonial.DeletedAt = DateTime.Now;
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Testimonial has been deleted",
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
