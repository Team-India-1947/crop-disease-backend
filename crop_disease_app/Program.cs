using System.Net;
using hackathon_template.Config;
using hackathon_template.Controllers;
using hackathon_template.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(co =>
{
    co.AddDefaultPolicy(pb =>
    {
        pb.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Total-Count");
    });
});

ServiceBuilder.AddServices(builder);
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

AuthenticationConfig.AddAuthServices(builder);

builder.WebHost.UseStaticWebAssets();


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps("certificate.pfx", builder.Configuration.GetSection("CertificatePassword").Get<string>());
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

Routes.ConfigureRoutes(app);

app.MapRazorPages(); // Map Razor Pages// html support
app.UseDefaultFiles(); // To use index.html as default page
app.UseStaticFiles(); // To serve static files

app.MapIdentityApi<User>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

await AddRoles(app);

app.Run();

async Task AddRoles(WebApplication webApplication)
{
    // add roles
    using (var scope = webApplication.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = Enum.GetNames(typeof(UserRole));
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    // add main admin, client user, partner user
    using (var scope = webApplication.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        string email = "admin@admin.com";
        string password = "Test1234!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                FirstName = "admin",
                LastName = "user",
                UserName = email,
                Email = email
            };
            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, Enum.GetName(UserRole.admin)!);
            await userManager.AddToRoleAsync(user, Enum.GetName(UserRole.client)!);
        }
    }
}