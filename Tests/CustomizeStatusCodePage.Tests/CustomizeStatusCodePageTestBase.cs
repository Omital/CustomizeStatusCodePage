using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.TestBase;
using CustomizeStatusCodePage.Authorization.Users;
using CustomizeStatusCodePage.EntityFramework;
using CustomizeStatusCodePage.Migrations.SeedData;
using CustomizeStatusCodePage.MultiTenancy;
using CustomizeStatusCodePage.Users;
using Castle.MicroKernel.Registration;
using Effort;
using EntityFramework.DynamicFilters;

namespace CustomizeStatusCodePage.Tests
{
    public abstract class CustomizeStatusCodePageTestBase : AbpIntegratedTestBase<CustomizeStatusCodePageTestModule>
    {
        private DbConnection _hostDb;
        private Dictionary<int, DbConnection> _tenantDbs; //only used for db per tenant architecture

        protected CustomizeStatusCodePageTestBase()
        {
            //Seed initial data for host
            AbpSession.TenantId = null;
            UsingDbContext(context =>
            {
                new InitialHostDbBuilder(context).Create();
                new DefaultTenantCreator(context).Create();
            });

            //Seed initial data for default tenant
            AbpSession.TenantId = 1;
            UsingDbContext(context =>
            {
                new TenantRoleAndUserBuilder(context, 1).Create();
            });

            LoginAsDefaultTenantAdmin();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            /* You can switch database architecture here: */
            UseSingleDatabase();
            //UseDatabasePerTenant();
        }

        /* Uses single database for host and all tenants.
         */
        private void UseSingleDatabase()
        {
            _hostDb = DbConnectionFactory.CreateTransient();

            LocalIocManager.IocContainer.Register(
                Component.For<DbConnection>()
                    .UsingFactoryMethod(() => _hostDb)
                    .LifestyleSingleton()
                );
        }

        /* Uses single database for host and Default tenant,
         * but dedicated databases for all other tenants.
         */
        private void UseDatabasePerTenant()
        {
            _hostDb = DbConnectionFactory.CreateTransient();
            _tenantDbs = new Dictionary<int, DbConnection>();

            LocalIocManager.IocContainer.Register(
                Component.For<DbConnection>()
                    .UsingFactoryMethod((kernel) =>
                    {
                        lock (_tenantDbs)
                        {
                            var currentUow = kernel.Resolve<ICurrentUnitOfWorkProvider>().Current;
                            var abpSession = kernel.Resolve<IAbpSession>();

                            var tenantId = currentUow != null ? currentUow.GetTenantId() : abpSession.TenantId;

                            if (tenantId == null || tenantId == 1) //host and default tenant are stored in host db
                            {
                                return _hostDb;
                            }

                            if (!_tenantDbs.ContainsKey(tenantId.Value))
                            {
                                _tenantDbs[tenantId.Value] = DbConnectionFactory.CreateTransient();
                            }

                            return _tenantDbs[tenantId.Value];
                        }
                    }, true)
                    .LifestyleTransient()
                );
        }

        #region UsingDbContext

        protected IDisposable UsingTenantId(int? tenantId)
        {
            var previousTenantId = AbpSession.TenantId;
            AbpSession.TenantId = tenantId;
            return new DisposeAction(() => AbpSession.TenantId = previousTenantId);
        }

        protected void UsingDbContext(Action<CustomizeStatusCodePageDbContext> action)
        {
            UsingDbContext(AbpSession.TenantId, action);
        }

        protected Task UsingDbContextAsync(Func<CustomizeStatusCodePageDbContext, Task> action)
        {
            return UsingDbContextAsync(AbpSession.TenantId, action);
        }

        protected T UsingDbContext<T>(Func<CustomizeStatusCodePageDbContext, T> func)
        {
            return UsingDbContext(AbpSession.TenantId, func);
        }

        protected Task<T> UsingDbContextAsync<T>(Func<CustomizeStatusCodePageDbContext, Task<T>> func)
        {
            return UsingDbContextAsync(AbpSession.TenantId, func);
        }

        protected void UsingDbContext(int? tenantId, Action<CustomizeStatusCodePageDbContext> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CustomizeStatusCodePageDbContext>())
                {
                    context.DisableAllFilters();
                    action(context);
                    context.SaveChanges();
                }
            }
        }

        protected async Task UsingDbContextAsync(int? tenantId, Func<CustomizeStatusCodePageDbContext, Task> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CustomizeStatusCodePageDbContext>())
                {
                    context.DisableAllFilters();
                    await action(context);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected T UsingDbContext<T>(int? tenantId, Func<CustomizeStatusCodePageDbContext, T> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CustomizeStatusCodePageDbContext>())
                {
                    context.DisableAllFilters();
                    result = func(context);
                    context.SaveChanges();
                }
            }

            return result;
        }

        protected async Task<T> UsingDbContextAsync<T>(int? tenantId, Func<CustomizeStatusCodePageDbContext, Task<T>> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CustomizeStatusCodePageDbContext>())
                {
                    context.DisableAllFilters();
                    result = await func(context);
                    await context.SaveChangesAsync();
                }
            }

            return result;
        }

        #endregion

        #region Login

        protected void LoginAsHostAdmin()
        {
            LoginAsHost(User.AdminUserName);
        }

        protected void LoginAsDefaultTenantAdmin()
        {
            LoginAsTenant(Tenant.DefaultTenantName, User.AdminUserName);
        }

        protected void LogoutAsDefaultTenant()
        {
            LogoutAsTenant(Tenant.DefaultTenantName);
        }

        protected void LoginAsHost(string userName)
        {
            AbpSession.TenantId = null;

            var user =
                UsingDbContext(
                    context =>
                        context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for host.");
            }

            AbpSession.UserId = user.Id;
        }

        protected void LogoutAsHost()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            AbpSession.TenantId = null;
            AbpSession.UserId = null;
        }

        protected void LoginAsTenant(string tenancyName, string userName)
        {
            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant == null)
            {
                throw new Exception("There is no tenant: " + tenancyName);
            }

            AbpSession.TenantId = tenant.Id;

            var user =
                UsingDbContext(
                    context =>
                        context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
            }

            AbpSession.UserId = user.Id;
        }

        protected void LogoutAsTenant(string tenancyName)
        {
            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant == null)
            {
                throw new Exception("There is no tenant: " + tenancyName);
            }

            AbpSession.TenantId = tenant.Id;
            AbpSession.UserId = null;
        }

        #endregion

        /// <summary>
        /// Gets current user if <see cref="IAbpSession.UserId"/> is not null.
        /// Throws exception if it's null.
        /// </summary>
        protected async Task<User> GetCurrentUserAsync()
        {
            var userId = AbpSession.GetUserId();
            return await UsingDbContext(context => context.Users.SingleAsync(u => u.Id == userId));
        }

        /// <summary>
        /// Gets current tenant if <see cref="IAbpSession.TenantId"/> is not null.
        /// Throws exception if there is no current tenant.
        /// </summary>
        protected async Task<Tenant> GetCurrentTenantAsync()
        {
            var tenantId = AbpSession.GetTenantId();
            return await UsingDbContext(context => context.Tenants.SingleAsync(t => t.Id == tenantId));
        }
    }
}