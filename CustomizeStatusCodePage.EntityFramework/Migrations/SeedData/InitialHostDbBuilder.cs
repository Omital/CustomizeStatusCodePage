using CustomizeStatusCodePage.EntityFramework;
using EntityFramework.DynamicFilters;

namespace CustomizeStatusCodePage.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly CustomizeStatusCodePageDbContext _context;

        public InitialHostDbBuilder(CustomizeStatusCodePageDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
