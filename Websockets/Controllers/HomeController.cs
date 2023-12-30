using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Websockets.Models;

namespace Websockets.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
