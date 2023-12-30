using EdgeCut.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EdgeCut.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Incorrect Credentials");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password,false,false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect Credentials");
                return View();
            }
            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
        }

        //[HttpGet("/create-user")]
        //public async Task<IActionResult> CreateUser()
        //{
        //    IdentityUser user = new IdentityUser
        //    {
        //        Email = "admin@admin.com",
        //        UserName = "admin"
        //    };
        //    IdentityResult identityResult = await _userManager.CreateAsync(user, "Admin123");
        //    if (!identityResult.Succeeded) return Json(identityResult.Errors.Select(x => x.Description));
        //    return Content("ok");
        //}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
