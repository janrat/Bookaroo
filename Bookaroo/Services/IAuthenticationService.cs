using System.Security.Claims;

namespace Bookaroo.Services
{
    public interface IAuthenticationService
    {
        bool ValidateToken(string token, out ClaimsPrincipal principal);
        bool ValidateUserRole(string token, string role);
    }
}
