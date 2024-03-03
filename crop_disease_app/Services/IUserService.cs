using hackathon_template.Controllers;
using hackathon_template.Models;

namespace hackathon_template.Services;

public interface IUserService {
    User? GetUserById(string userId);
    public Task AddRole(User user, UserRole role);
    IEnumerable<string> GetRoles(string userId);
    public Task SetupUserAccount(UserRegistrationDto userDto);
    List<Recipe> GetUserRecipes(string userId);
    Recipe? GetUserRecipeById(string userId, int recipeId);
    Recipe? UpdateRecipeById(string userId, int recipeId, RecipePutDto request);
    Recipe? CreateRecipe(string userId, RecipePostDto request);
    void DeleteRecipeById(string userId, int recipeId);
    void StoreDisease(string userId, string requestDiseaseName, double requestLatitude, double requestLongtitude, string url);
    List<Alert> GetAlerts(DateTimeOffset dateFrom, double latitude, double longitude, double radius);
    void StoreAlert(string pest, string userId, long timestamp, double latitude, double longitude);
    void WipeDatabase();
    void SetUserSettings(UserSettings userSettings, string userId);
}