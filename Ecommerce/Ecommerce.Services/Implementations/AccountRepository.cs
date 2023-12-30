using Ecommerce.Data.DTOs.UserDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailRepository _emailRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _accessor;
        public AccountRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailRepository emailRepository,
            ApplicationContext context,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailRepository = emailRepository;
            _context = context;
            _roleManager = roleManager;
            _accessor = accessor;
        }

        public async Task<(bool, string)> ActivateUser(string token)
        {
            AppUser user = await _context.AppUsers.FirstOrDefaultAsync(x => x.ActivateToken == token);
            if (user is null) return (false, "User not found");
            user.ActivateToken = "";
            user.IsActive = true;
            await _context.SaveChangesAsync();
            return (true, "Your account has been activated");
        }

        public async Task<(bool, string)> CreateRole(string role)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(role);
            if (roleExists) return (false, "This role is already exists");
            await _roleManager.CreateAsync(new IdentityRole(role));
            return (true, "Role has been created successfully");
        }

        public async Task<(bool, string)> Login(LoginDTO model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return (false, "Incorrect Credentials");
            if(!user.IsActive) return (false, "Incorrect Credentials");
            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password,true,false);
            if (!result.Succeeded) return (false, "Incorrect Credentials");

            return (true, "Successfully logged in");
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool, string)> Register(RegisterDTO model)
        {
            AppUser user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null) return (false, "This user is already exists");
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) return (false, "This user is already exists");
            user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                ActivateToken = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return (false, result.Errors.Select(x=>x.Description).FirstOrDefault());
            await _userManager.AddToRoleAsync(user, "member");
            await _emailRepository.SendEmail(model.Email, "Activate your account", $"<a href=\"https://localhost:7035/account/activate/{user.ActivateToken}\" target=\"blank\" >Click</a> for activate your account");
            return (true, "Successfully registered");
        }

        public async Task CreateAdmin()
        {
            AppUser user = new AppUser
            {

                Email = "admin@gmail.com",
                UserName = "admin",
                IsActive = true,
            };

            await _userManager.CreateAsync(user, "Admin123");
            await _userManager.AddToRoleAsync(user, "admin");
        }

        public async Task<(bool, string)> UpdateProfile(ProfileDTO model)
        {
            AppUser user = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);
            if(!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword) return (false, "Please, confirm password");
                bool checkPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                if (!checkPassword) return (false, "Incorrect password");
                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded) return (false, result.Errors.Select(x => x.Description).FirstOrDefault());
            }

            user.Firstname = model.FirstName;
            user.Lastname = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Address = model.Address;
            user.Phone = model.Phone;
            await _userManager.UpdateAsync(user);
            return (true, "Profile has been updated successfully");
        }
    }
}
