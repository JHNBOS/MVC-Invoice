using InvoiceApplication.Data;
using InvoiceApplication.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private AppSettings settings;
        private ApplicationDbContext _context;

        public AuthMessageSender()
        {
            settings = _context.Settings.Single(s => s.ID == 1);
        }

        public async Task SendUserEmailAsync(string email, string pass)
        {
            string smtp = settings.SMTP;
            int port = settings.Port;
            string company = settings.CompanyName;
            string company_email = settings.Email;
            string password = settings.Password;
            string website = settings.Website;

            string subject = "Inloggevens " + company;
            string message = "Geachte heer, mevrouw,"
                            + "\n\n"
                            + "Hierbij ontvangt u van ons uw inloggevens. Hiermee kunt u inloggen om uw facturen te bekijken en/of te betalen."
                            + "\n\n"
                            + "Gebruikersnaam:\t\t" + email
                            + "\n"
                            + "Wachtwoord:\t\t" + pass
                            + "\n\n"
                            + "U kunt inloggen op " + "http://" + website
                            + "\n\n\n"
                            + "Met vriendelijke groet,"
                            + "\n\n"
                            + company;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(company, company_email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(smtp, port, false).ConfigureAwait(false);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(company_email, password)
                    .ConfigureAwait(false);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
        }

        public async Task SendInvoiceEmailAsync(string email)
        {
            string smtp = settings.SMTP;
            int port = settings.Port;
            string company = settings.CompanyName;
            string company_email = settings.Email;
            string password = settings.Password;
            string website = settings.Website;

            string subject = "Factuur beschikbaar op " + company;
            string message = "Er staat een factuur voor u klaar op " + company + "."
                            + "\n"
                            + "http://" + website
                            + "\n\n\n"
                            + "Met vriendelijke groet,"
                            + "\n\n"
                            + company;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(company, company_email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(smtp, port, false).ConfigureAwait(false);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(company_email, password)
                    .ConfigureAwait(false);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        
    }
}
