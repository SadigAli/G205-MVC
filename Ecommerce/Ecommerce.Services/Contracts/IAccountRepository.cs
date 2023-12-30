using Ecommerce.Data.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IAccountRepository
    {
        public Task<(bool, string)> Register(RegisterDTO model);
        public Task<(bool, string)> Login(LoginDTO model);
        public Task<(bool, string)> ActivateUser(string token);
        public Task<(bool, string)> CreateRole(string role);
        public Task<(bool, string)> UpdateProfile(ProfileDTO model);
        public Task CreateAdmin();
        public Task Logout();
    }
}
