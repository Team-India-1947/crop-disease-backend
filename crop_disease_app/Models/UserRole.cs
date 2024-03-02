namespace hackathon_template.Models; 

public enum UserRole {
    admin,
    client
}

public static class UserRoleExtensions {
    public static IEnumerable<UserRole> StringsToRoles(IEnumerable<string> input) {
        foreach (string roleString in input) {
            switch (roleString) {
                case "client":
                    yield return UserRole.admin;
                    break;
                case "admin":
                    yield return UserRole.admin;
                    break;
                default:
                    throw new ArgumentException("Unknown UserRole");
            }
        }
    }
}