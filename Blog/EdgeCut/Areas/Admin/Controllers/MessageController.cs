using EdgeCut.DAL;
using EdgeCut.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationContext _context;
        public MessageController(ApplicationContext context)
        {
            _context = context;    
        }
        public IActionResult Index()
        {
            List<Message> messages = _context.Messages.ToList();
            return View(messages);
        }

        public IActionResult Details(int id)
        {
            Message message = _context.Messages.FirstOrDefault(m => m.Id == id);
            if (message is null) return NotFound();
            message.IsRead = true;
            _context.SaveChanges();
            return View(message);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Message message = _context.Messages.FirstOrDefault(m => m.Id == id);
                if (message is null) return NotFound();
                _context.Messages.Remove(message);
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Message deleted successfully",
                    Status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = ex.Message,
                    Status = true
                });
            }
        }
    }
}
