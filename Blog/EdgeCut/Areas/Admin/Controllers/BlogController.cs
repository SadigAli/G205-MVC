using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class BlogController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;
        public BlogController(ApplicationContext context,IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        // GET: BlogController
        public ActionResult Index()
        {
            List<Blog> blogs = _context.Blogs.Where(x=>x.DeletedAt == null).ToList();
            return View(blogs);
        }

        // GET: BlogController/Details/5
        public ActionResult Details(int id)
        {
            Blog blog = _context.Blogs
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
            if(blog is null) return NotFound(); 
            return View(blog);
        }

        // GET: BlogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Blog blog)
        {
            try
            {
                if (blog.File is null) ModelState.AddModelError("File", "File is required");
                (int status, string message) = await _fileService.FileUpload("blogs", blog.File);
                if (status == 0) ModelState.AddModelError("File", message);
                if (!ModelState.IsValid) return View(blog);
                blog.Image = message;
                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Blog has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: BlogController/Edit/5
        public ActionResult Edit(int id)
        {
            Blog blog = _context.Blogs
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
            if (blog is null) return NotFound();

            return View(blog);
        }

        // POST: BlogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Blog model)
        {
            try
            {
                Blog blog = _context.Blogs
                .Where(x => x.DeletedAt == null)
                .FirstOrDefault(x => x.Id == id);
                if (blog is null) return NotFound();
                if(model.File  != null)
                {
                    (int status, string message) = await _fileService.FileUpload("blogs", model.File);
                    if (status == 0) ModelState.AddModelError("File", message);
                    if (!ModelState.IsValid) return View(model);
                    _fileService.DeleteFile("blogs", blog.Image);
                    blog.Image = message;
                }
                blog.Title = model.Title;
                blog.Description = model.Description;
                blog.UpdatedAt = DateTime.Now;
                _context.SaveChangesAsync();
                TempData["Message"] = "Blog has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: BlogController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Blog blog = _context.Blogs
                            .Where(x => x.DeletedAt == null)
                            .FirstOrDefault(x => x.Id == id);
                if (blog is null) return NotFound();
                blog.DeletedAt = DateTime.Now;
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Blog has been deleted",
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
