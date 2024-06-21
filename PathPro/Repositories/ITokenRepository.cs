using Microsoft.AspNetCore.Identity;

namespace PathPro.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
