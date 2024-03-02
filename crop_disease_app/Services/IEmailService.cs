namespace hackathon_template.Services;

public interface IEmailService {
    void SendEmail(string toEmail, string subject, string body);
}