using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ToDoWeb.Infrastructure
{
    /// <summary>
    /// Send Email
    /// </summary>
    public static class EmailService
    {
        public static void SendMail(MailAddress to, string subject, string body)
        {
            MailMessage message = new MailMessage();

            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
    }
}
