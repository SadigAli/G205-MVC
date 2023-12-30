using EdgeCut.DAL;
using EdgeCut.Models;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationContext _context;
        public ContactController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Page = "Contact";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChangesAsync();
            string redirect = Request.Headers["Referer"].ToString();
            TempData["Message"] = "Message has been sent successfully";
            return Redirect(redirect);
        }
    }
}
