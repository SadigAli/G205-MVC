using Ecommerce.Data.DTOs.UserDTO;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            (bool status,string message) = await _accountRepository.Login(login);
            if (!status)
            {
                ModelState.AddModelError("", message);
                return View();
            }

            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await _accountRepository.Logout();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
