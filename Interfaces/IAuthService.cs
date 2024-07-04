using MongoDataAccess.Models;

namespace MovieBackend.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<bool> Login(User user);
        Task<string> GenerateTokenAsync(User user);
        Task<bool> AssignRole(UserRoleAssignment userRoleAssignment);
    }
}
