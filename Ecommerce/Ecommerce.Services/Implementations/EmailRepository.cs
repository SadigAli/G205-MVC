using Ecommerce.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Ecommerce.Services.Implementations.EmailRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce.Services.Implementations
{
    public class EmailRepository : IEmailRepository
    {
        public async Task SendEmail(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("sadig.aliev99@gmail.com", "lboncarqclyrzdhj"),
            };
            MailMessage mailMessage = new MailMessage(from: "sadig.aliev99@gmail.com", to: email, subject, message);
            mailMessage.IsBodyHtml = true;
            await client.SendMailAsync(mailMessage);
        }

    }
}
