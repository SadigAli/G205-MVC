using Ecommerce.Data.DTOs.UserDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            (bool status,string message) = await _accountRepository.Register(register);
            if(!status)
            {
                ModelState.AddModelError("", message);
                return View(register);
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            (bool status, string message) = await _accountRepository.Login(login);
            if (!status)
            {
                ModelState.AddModelError("", message);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("/account/activate/{token}")]
        public async Task<IActionResult> Activate(string token)
        {
            (bool status,string message) = await _accountRepository.ActivateUser(token);
            if (!status) return NotFound();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UpdateProfile(ProfileDTO profile)
        {
            (bool status, string message) = await _accountRepository.UpdateProfile(profile);
            if (!status)
            {
                ModelState.AddModelError("", message);
                return View("Profile");
            }
            TempData["Message"] = message;
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Logout()
        {
            await _accountRepository.Logout();
            return RedirectToAction("Index","Home");
        }

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    await _accountRepository.CreateAdmin();
        //    return Content("ok");
        //}

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _accountRepository.CreateRole("admin");
        //    await _accountRepository.CreateRole("member");
        //    return Content("ok");
        //}
    }
}
