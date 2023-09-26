using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using CustomizeStatusCodePage.Authorization.Users;
using CustomizeStatusCodePage.MultiTenancy;
using CustomizeStatusCodePage.Users;
using Microsoft.AspNet.Identity;

namespace CustomizeStatusCodePage
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class CustomizeStatusCodePageAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected CustomizeStatusCodePageAppServiceBase()
        {
            LocalizationSourceName = CustomizeStatusCodePageConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}