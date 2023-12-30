using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IEmailRepository
    {
        public Task SendEmail(string email, string subject, string message);
    }
}
