using indra.Web.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace indra.Web.Service
{
    public class EmailSender : IEmailSender
    {
        public readonly IConfiguration _config;
        private readonly EmailSettings _emailSettings;
        private string domain = "brunocdasilva12@gmail.com";
        
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public Task EnviaEmailAsync(string email, string assunto, string mensagem)
        {
            try
            {
                Execute(email, assunto, mensagem).Wait();
                return Task.FromResult(0);
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(string email, string assunto, string mensagem)
        {
            try
            {
                var ToEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Gestão de Estética")
                };
                mail.To.Add(new MailAddress(ToEmail));
                foreach(var item in _emailSettings.CcEmail)
                {
                    mail.CC.Add(new MailAddress(item));
                }

                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }

            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
