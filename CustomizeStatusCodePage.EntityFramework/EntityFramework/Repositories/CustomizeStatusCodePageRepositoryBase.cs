using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CustomizeStatusCodePage.EntityFramework.Repositories
{
    public abstract class CustomizeStatusCodePageRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CustomizeStatusCodePageDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected CustomizeStatusCodePageRepositoryBase(IDbContextProvider<CustomizeStatusCodePageDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class CustomizeStatusCodePageRepositoryBase<TEntity> : CustomizeStatusCodePageRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CustomizeStatusCodePageRepositoryBase(IDbContextProvider<CustomizeStatusCodePageDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
