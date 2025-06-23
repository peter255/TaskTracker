using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this System.Security.Claims.ClaimsPrincipal user)
        {
            int.TryParse(user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId);
            return userId;
        }
        public static string GetUserName(this System.Security.Claims.ClaimsPrincipal user)
        {
            return user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        }
        public static string GetEmail(this System.Security.Claims.ClaimsPrincipal user)
        {
            return user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        }
    }
}
