using EdgeCut.DAL;
using EdgeCut.Models;
using EdgeCut.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Page = "Home";
            List<Slider> sliders = _context.Sliders
                        .Where(x=>x.DeletedAt == null)
                        .ToList();
            List<Furniture> furnites = _context.Furnitures
                        .Where(x=>x.DeletedAt == null)
                        .ToList();
            List<Blog> blogs = _context.Blogs
                        .Where(x=>x.DeletedAt == null)
                        .ToList();
            List<Testimonial> testimonials = _context.Testimonials
                        .Where(x=>x.DeletedAt == null)
                        .ToList();
            HomeVM model = new HomeVM
            {
                Testimonials = testimonials,
                Blogs   = blogs,
                Furnitures = furnites,
                Sliders = sliders
            };
            return View(model);
        }
    }
}
