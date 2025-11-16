using Domain.Enums;
using System.Security.Claims;

namespace BankSystem.Helpers
{
    public static class PermissionHelper
    {
        public static bool HasPermission(ClaimsPrincipal user, UserPermissions permission)
        {
            if (!user.Identity?.IsAuthenticated ?? true)
                return false;

            var claim = user.Claims.FirstOrDefault(c => c.Type == "Permissions")?.Value;
            if (claim == null) return false;

            if (int.TryParse(claim, out int value))
            {
                var userPerm = (UserPermissions)value;
                return userPerm.HasFlag(permission);
            }

            return false;
        }
    }
}
