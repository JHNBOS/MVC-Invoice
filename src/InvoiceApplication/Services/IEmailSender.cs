using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public interface IEmailSender
    {
        //Task SendEmailAsync(string email, string subject, string message);
        Task SendUserEmailAsync(string email, string password);
        Task SendInvoiceEmailAsync(string email);

    }
}
