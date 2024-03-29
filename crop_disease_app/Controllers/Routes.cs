using hackathon_template.Models;
using hackathon_template.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace hackathon_template.Controllers;

public static class Routes {
    public static void ConfigureRoutes(IEndpointRouteBuilder app) {
        SharedApi(app);
        RecipesApi(app);
        DiseaseApi(app);
    }
    private static void SharedApi(IEndpointRouteBuilder app) {
        app.MapGet("/test", () => {
            var data = new { Message = "Hello, World!", Timestamp = DateTime.UtcNow };
            return data;
        });

        app.MapGet("/wipe-users", (IUserService userService) => {
            userService.WipeDatabase();
            return Results.Ok();
        });

        app.MapPost("/user-registered",
            async (UserRegistrationDto userDto, IUserService userService) => {
                await userService.SetupUserAccount(userDto);
            });

        app.MapPost("/logout", async (HttpContext httpContext, SignInManager<User> signInManager) => {
            await signInManager.SignOutAsync();
            return Results.Redirect("/");
        });

        app.MapGet("/user-info",
            async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
                var user = await userManager.GetUserAsync(httpContext.User);
                if (user is null) {
                    return Results.Unauthorized();
                }

                return Results.Json(user);
            });

        app.MapGet("/test-email", (IEmailService emailService) => {
            string html = "<h1 style=\"color: green;\">header</h1><p>p tag!</p>";
            emailService.SendEmail("m95150599@gmail.com", "test subject!", html);
            return Results.Ok();
        });
    }

    private static void RecipesApi(IEndpointRouteBuilder app) {
        app.MapGet("/recipes", async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) {
                return Results.Unauthorized();
            }

            List<Recipe> recipes = userService.GetUserRecipes(user.Id);
            httpContext.Response.Headers["X-Total-Count"] = recipes.Count.ToString();
            return Results.Json(recipes);
        });

        app.MapGet("/recipes/{recipeId}",
            async (int recipeId, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
                var user = await userManager.GetUserAsync(httpContext.User);
                if (user is null) {
                    return Results.Unauthorized();
                }

                return Results.Json(userService.GetUserRecipeById(user.Id, recipeId));
        });
        
        app.MapPost("/recipes", async (RecipePostDto request, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) {
                return Results.Unauthorized();
            }
            
            return Results.Json(userService.CreateRecipe(user.Id, request));
        });
        
        app.MapPut("/recipes/{recipeId}", async (int recipeId, RecipePutDto request, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }
            
            return Results.Json(userService.UpdateRecipeById(user.Id, recipeId, request));
        });

        app.MapDelete("/recipes/{recipeId}", async (int recipeId, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }
            
            userService.DeleteRecipeById(user.Id, recipeId);
            return Results.Ok();
        });
    }
    private static void DiseaseApi(IEndpointRouteBuilder app) {
        app.MapPost("/store-disease-datapoint", async (HttpRequest request, IUserService userService, IBlobService blobService, UserManager<User> userManager, HttpContext httpContext) => {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest("Unsupported media type");
            }
            
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }
            
            var form = await request.ReadFormAsync();
            if (!form.Files.Any())
            {
                return Results.BadRequest("No files uploaded.");
            }
            var dto = new StoreDiseaseDto
            (form["disease_name"], double.Parse(form["latitude"]),
                double.Parse(form["longitude"]));
            var imageFile = form.Files["file"];

            string url = await blobService.StoreImage(imageFile);
            userService.StoreDisease(user.Id, dto.DiseaseName, dto.Latitude, dto.Longitude, url);
            
            return Results.Ok();
        });

        app.MapGet("/get-alerts", async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }

            var dateFrom = DateTimeOffset.UtcNow.AddHours(-user.userSettings.AlertsFromInDays);
            var latitude = user.userSettings.Latitude;
            var longitude = user.userSettings.Longitude;
            var radius = user.userSettings.AlertRadius;

            return Results.Json(userService.GetAlerts(dateFrom, latitude, longitude, radius));
        });

        app.MapPost("/send-alert", async (string pest, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }

            userService.StoreAlert(pest, user.Id, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), user.userSettings.Latitude, user.userSettings.Longitude);
            return Results.Ok();
        });

        app.MapPost("/set-user-settings", async (UserSettings userSettings, IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }
            
            userService.SetUserSettings(userSettings, user.Id);
            return Results.Ok();
        });

        app.MapGet("/get-disease-data", async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }
            
            return Results.Json(userService.GetUserDiseases(user.Id));
        });
        
        app.MapGet("/get-diseases-for-user", async (IUserService userService, UserManager<User> userManager, HttpContext httpContext) => {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user is null) { 
                return Results.Unauthorized();
            }

            var latitude = user.userSettings.Latitude;
            var longitude = user.userSettings.Longitude;
            var radius = user.userSettings.AlertRadius;

            return Results.Json(userService.GetDiseasesForUser( latitude, longitude, radius));
        });
    }
}

public record UserRegistrationDto(string FirstName, string LastName, string Email);
public record RecipePostDto(string title, string body);
public record RecipePutDto(string title, string body);
public record StoreDiseaseDto(string DiseaseName, double Latitude, double Longitude);
