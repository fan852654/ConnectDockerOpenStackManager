using System.Net;
using System.Net.Mail;

namespace Docker
{
    class EmailSender
    {
        public static bool SendEmail(string email, string subject, string message)
        {
            var fromaddress = new MailAddress("ghost980@outlook.com");
            var toaddress = new MailAddress(email);
            const string frompassword = "fanzs900839";
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromaddress.Address, frompassword)
            };
            using (var messages = new MailMessage(fromaddress, toaddress)
            {

                Subject = subject + "验证邮箱",
                Body = message
            })
                try
                {
                    smtp.Send(messages);
                    return true;
                }
                catch
                {
                    return false;
                }
        }
    }
}
