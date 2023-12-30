using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class SettingController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFileService _fileService;
        public SettingController(ApplicationContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Settings.FirstOrDefault();
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Setting model)
        {
            Setting setting = _context.Settings.FirstOrDefault();
            if(setting == null)
            {
                if (model.File is null) ModelState.AddModelError("File", "File is required");
                if (!ModelState.IsValid) return View("Index", model);
                (int status, string message) = await _fileService.FileUpload("settings", model.File);
                if (status == 0) ModelState.AddModelError("File", message);
                if (!ModelState.IsValid) return View("Index", model);
                model.AboutImage = message;
                _context.Settings.Add(model);
            }
            else
            {
                if(model.File != null)
                {
                    if (model.File is null) ModelState.AddModelError("File", "File is required");
                    if (!ModelState.IsValid) return View("Index", model);
                    (int status, string message) = await _fileService.FileUpload("settings", model.File);
                    if (status == 0) ModelState.AddModelError("File", message);
                    if (!ModelState.IsValid) return View("Index", model);
                    _fileService.DeleteFile("settings", setting.AboutImage);
                    setting.AboutImage = message;
                }
                setting.Email = model.Email;
                setting.Phone = model.Phone;
                setting.Location = model.Location;
                setting.AboutTitle = model.AboutTitle;
                setting.AboutDesc = model.AboutDesc;
            }
            await _context.SaveChangesAsync();
            TempData["Message"] = "Settings has been updated successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
