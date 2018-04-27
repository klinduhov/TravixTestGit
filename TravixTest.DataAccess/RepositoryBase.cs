using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{
    public abstract class RepositoryBase<TModel, TEntity> : IRepository<TModel> 
        where TModel : IDomainModel
        where TEntity : class, IEntity
    {
        private readonly DbContextOptions<PostsCommentsContext> dbOptions;

        protected RepositoryBase(DbContextOptions<PostsCommentsContext> dbOptions)
        {
            this.dbOptions = dbOptions;
        }

        protected abstract TModel MapEntityToModel(TEntity entity);
        protected abstract TEntity MapModelToEntity(TModel model);

        protected PostsCommentsContext GenerateContext()
        {
            return new PostsCommentsContext(dbOptions);
        }

        protected async Task DeleteAsync(TModel model, Func<TModel, TEntity> withIdEntityConstructor)
        {
            using (var db = GenerateContext())
            {
                var entityForDelete = withIdEntityConstructor(model);
                db.Attach(entityForDelete);
                db.Remove(entityForDelete);
                 await db.SaveChangesAsync();
            }
        }

        public abstract Task DeleteAsync(TModel model);
        public abstract Task<TModel> GetAsync(Guid id);
        public abstract Task<IEnumerable<TModel>> GetAllASync();        

        public async Task AddAsync(TModel model)
        {
            using (var db = GenerateContext())
            {
                TEntity entity = MapModelToEntity(model);
                db.Add(entity);
                await db.SaveChangesAsync();
            }
        }
    }
}
