using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApp.Entity;

namespace WebApp.Helpers
{
    public class AppUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<User, IdentityRole<int>>
    {
        public AppUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var loginClaim = identity.Claims
                .FirstOrDefault(c => c.Type == "Login");

            if (loginClaim != null)
            {
                identity.RemoveClaim(loginClaim);
            }

            identity.AddClaim(new Claim("Login", user.Login ?? string.Empty));

            return identity;
        }


    }

}
