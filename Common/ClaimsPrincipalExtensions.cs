using System.Security.Claims;

namespace TaskApp.Api.Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID claim is missing");

            return Guid.Parse(userId);
        }
    }
}
