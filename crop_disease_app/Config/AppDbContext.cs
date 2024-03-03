using hackathon_template.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hackathon_template.Config; 

public class AppDbContext : IdentityDbContext<User> {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<User>().ComplexProperty(m => m.UserSettings);
    }

    public DbSet<Recipe> SavedRecipes { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<Alert> Alerts { get; set; }
}