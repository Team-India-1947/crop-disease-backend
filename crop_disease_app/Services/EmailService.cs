using System.Net;
using System.Net.Mail;

namespace hackathon_template.Services; 

public class EmailService : IEmailService {
        private readonly string fromEmail = "m95150599@gmail.com";
    private readonly string fromName = "Team India";
    private readonly string fromPassword = "tuxd rcje ioxw ikej";
    
    public void SendEmail(string toEmail, string subject, string body) {
        var fromAddress = new MailAddress(fromEmail, fromName);
        var toAddress = new MailAddress(toEmail, subject);
        
        var smtp = new SmtpClient {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        
        using (var message = new MailMessage(fromAddress, toAddress) {
                   Subject = subject,
                   Body = body,
                   IsBodyHtml = true
               }) {
            smtp.Send(message);
        }
    }
}