using hackathon_template.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace hackathon_template.Config; 

public static class AuthenticationConfig {
    public static void AddAuthServices(WebApplicationBuilder builder) {
        var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        builder.Services.AddDbContext<AppDbContext>(x =>
            x.UseNpgsql(databaseSettings.ConnectionString));
        
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("admin", policy =>
            policy.RequireRole("admin"));
        
        builder.Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
    }
}