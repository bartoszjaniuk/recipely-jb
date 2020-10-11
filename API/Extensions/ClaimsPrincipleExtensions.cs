using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; THE SAME
        public static string GetUsername (this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}