using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using CustomizeStatusCodePage.EntityFramework;

namespace CustomizeStatusCodePage.Migrator
{
    [DependsOn(typeof(CustomizeStatusCodePageDataModule))]
    public class CustomizeStatusCodePageMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<CustomizeStatusCodePageDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}