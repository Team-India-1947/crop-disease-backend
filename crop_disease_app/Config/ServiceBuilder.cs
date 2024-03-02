using hackathon_template.Services;

namespace hackathon_template.Config; 

public static class ServiceBuilder {
    public static void AddServices(WebApplicationBuilder builder) {
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}