using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Domain.Enums;

public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserPermissions _permission;

    public PermissionAttribute(UserPermissions permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var empFlag = context.HttpContext.User.FindFirst("IsEmployee");
        if (empFlag == null)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        var claim = context.HttpContext.User.FindFirst("Permissions");
        if (claim == null)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        int userPerm = int.Parse(claim.Value);

        if ((userPerm & (int)_permission) != (int)_permission)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
        }
    }
}
