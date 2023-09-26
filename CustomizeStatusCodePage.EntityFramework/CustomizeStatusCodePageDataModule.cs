using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using CustomizeStatusCodePage.EntityFramework;

namespace CustomizeStatusCodePage
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(CustomizeStatusCodePageCoreModule))]
    public class CustomizeStatusCodePageDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<CustomizeStatusCodePageDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
