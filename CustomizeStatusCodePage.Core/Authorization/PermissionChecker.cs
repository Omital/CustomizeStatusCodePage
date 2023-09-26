using Abp.Authorization;
using CustomizeStatusCodePage.Authorization.Roles;
using CustomizeStatusCodePage.Authorization.Users;

namespace CustomizeStatusCodePage.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
