using System.Net.Mail;
using System.Net;

namespace server.Infrastructure
{
    public class EmailSender
    {
        public void SendEmail(string recepient, string token)
        {
            var fromAddress = new MailAddress("tbhzone@gmail.com", "TBH Zone");
            var toAddress = new MailAddress(recepient, "Customer");

            const string subject = "Noreply";
            string body = "Please click here to activate your account:\nhttp://localhost:5000/verify/" + token;
            

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server hostname
                smtpClient.Port = 587; // Replace with the appropriate port number
                smtpClient.EnableSsl = true; // Set to true if your SMTP server requires SSL
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("tbhzone@gmail.com", "jgfgjieklheusjlf");

                using (var mailMessage = new MailMessage(fromAddress, toAddress))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    smtpClient.Send(mailMessage);
                }
                Console.WriteLine("Sent");
            }
        }

    }
}