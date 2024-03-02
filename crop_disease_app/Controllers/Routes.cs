using hackathon_template.Models;
using hackathon_template.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace hackathon_template.Controllers;

public static class Routes {
    public static void ConfigureRoutes(IEndpointRouteBuilder app) {
        app.MapGet("/test", () => {
            var data = new { Message = "Hello, World!", Timestamp = DateTime.UtcNow };
            return data;
        });
        
        app.MapPost("/user-registered", async (UserRegistrationDto userDto, IUserService userService) => {
            await userService.SetupUserAccount(userDto);
        });
        
        app.MapPost("/logout", async (HttpContext httpContext, SignInManager<User> signInManager) => {
            await signInManager.SignOutAsync();
            return Results.Redirect("/");
        });

        app.MapGet("/user-info", 
            async (IUserService userService, UserManager<User> userManager, HttpContext httpContext)  => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) {
                return Results.NotFound("user not found");
            }
            return Results.Json(new {user.FirstName, user.LastName, user.Email});
        });

        app.MapGet("/test-email", (IEmailService emailService) => {
            string html = "<h1 style=\"color: green;\">header</h1><p>p tag!</p>";
            emailService.SendEmail("m95150599@gmail.com", "test subject!", html);
            return Results.Ok();
        });

        app.MapGet("/recipes", async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.NotFound("user not found"); 
            }
            
            return Results.Json(userService.GetUserRecipes(user.Id));
        });
        
        app.MapGet("/recipes/{recipeId}", async (string recipeId, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.NotFound("user not found"); 
            }
            
            return Results.Json(userService.GetUserRecipes(user.Id));
        });
    }
}

public record UserRegistrationDto(string FirstName, string LastName, string Email);