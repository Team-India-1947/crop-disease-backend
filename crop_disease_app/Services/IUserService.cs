using hackathon_template.Controllers;
using hackathon_template.Models;

namespace hackathon_template.Services;

public interface IUserService {
    User? GetUserById(string userId);
    public Task AddRole(User user, UserRole role);
    IEnumerable<string> GetRoles(string userId);
    public Task SetupUserAccount(UserRegistrationDto userDto);
}