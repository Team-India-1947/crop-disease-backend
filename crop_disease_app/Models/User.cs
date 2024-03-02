using Microsoft.AspNetCore.Identity;

namespace hackathon_template.Models;

public class User : IdentityUser {
    public DateTime CreatedOn { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Recipe> SavedRecipes { get; set; }

    public User() :base() {
        CreatedOn = DateTime.UtcNow;
    }
}