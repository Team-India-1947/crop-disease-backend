using hackathon_template.Config;
using hackathon_template.Controllers;
using hackathon_template.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace hackathon_template.Services; 

public class UserService : IUserService {
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserService(AppDbContext context, UserManager<User> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public User? GetUserById(string userId) {
        return _context.Users.Find(userId);
    }
    
    public async Task AddRole(User user, UserRole role) {
        await _userManager.AddToRoleAsync(user, Enum.GetName(role)!);
    }

    public IEnumerable<string> GetRoles(string userId) {
        return _context.Roles
                .AsNoTracking()
                .Where(role => _context.UserRoles.Any(ur => 
                ur.RoleId == role.Id
                && ur.UserId == userId))
                .Select(role => role.Name!); 
    }

    public async Task SetupUserAccount(UserRegistrationDto userDto) {
        User? user = await _userManager.FindByEmailAsync(userDto.Email);
        if (user is null) {
            return;
        }
        await AddRole(user, UserRole.client);
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        await _context.SaveChangesAsync();
    }

    public IEnumerable<User> GetAllUsersWithRole(UserRole inputRole) {
        // query all users whose roles are the required role
        return _context.Users
            .AsNoTracking()
            .Where(user => _context.UserRoles
                .Any(userRole => userRole.UserId == user.Id
                                 && _context.Roles
                                     .Any(role =>
                                         role.Id == userRole.RoleId
                                         && role.Name == Enum.GetName(inputRole))))
            .OrderBy(user => user.UserName);
    }

    public List<Recipe> GetUserRecipes(string userId) {
        return _context.Users.Include(u => u.SavedRecipes)
            .First(u => u.Id == userId).SavedRecipes;
    }
}