using System.Linq;
using CustomizeStatusCodePage.EntityFramework;
using CustomizeStatusCodePage.MultiTenancy;

namespace CustomizeStatusCodePage.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly CustomizeStatusCodePageDbContext _context;

        public DefaultTenantCreator(CustomizeStatusCodePageDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
