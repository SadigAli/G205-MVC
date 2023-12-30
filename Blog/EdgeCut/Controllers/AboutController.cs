using EdgeCut.DAL;
using EdgeCut.Models;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationContext _context;
        
        public AboutController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Page = "About";
            Setting setting = _context.Settings.FirstOrDefault();
            return View(setting);
        }
    }
}
