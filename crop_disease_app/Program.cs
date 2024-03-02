using hackathon_template.Config;
using hackathon_template.Controllers;
using hackathon_template.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(co => {
    co.AddDefaultPolicy(pb => {
        pb.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

ServiceBuilder.AddServices(builder);
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

AuthenticationConfig.AddAuthServices(builder);

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

Routes.ConfigureRoutes(app);

app.MapRazorPages(); // Map Razor Pages// html support
app.UseDefaultFiles(); // To use index.html as default page
app.UseStaticFiles(); // To serve static files

app.MapIdentityApi<User>();
await AddRoles(app);

app.Run();

async Task AddRoles(WebApplication webApplication) {
// add roles
    using (var scope = webApplication.Services.CreateScope()) {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = Enum.GetNames(typeof(UserRole));
        foreach (var role in roles) {
            if (!await roleManager.RoleExistsAsync(role)) {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

// add main admin, client user, partner user
    using (var scope = webApplication.Services.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        string email = "admin@admin.com";
        string password = "Test1234!";

        if (await userManager.FindByEmailAsync(email) == null) {
            var user = new User {
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