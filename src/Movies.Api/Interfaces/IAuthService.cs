using Movies.Api.Domain;

namespace Movies.Api.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<(bool succeeded, ApplicationUser appUser, string token)> Login(User user);
        Task<bool> AssignRole(UserRoleAssignment userRoleAssignment);
    }
}
