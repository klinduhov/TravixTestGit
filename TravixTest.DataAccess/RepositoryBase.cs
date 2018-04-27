using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected void Delete(TModel model, Func<TModel, TEntity> withIdEntityConstructor)
        {
            AtomicModify(db =>
            {
                var entityForDelete = withIdEntityConstructor(model);
                db.Attach(entityForDelete);
                db.Remove(entityForDelete);
            });
        }

        public abstract IEnumerable<TModel> GetAll();
        public abstract void Delete(TModel model);
        public abstract TModel Get(Guid id);

        public void Add(TModel model)
        {
            AtomicModify(db =>
            {
                TEntity entity = MapModelToEntity(model);
                db.Add(entity);
            });
        }

        protected void AtomicModify(Action<PostsCommentsContext> dbUnitOfWorkSaveAction)
        {
            AtomicDbAction(db =>
            {
                dbUnitOfWorkSaveAction(db);
                db.SaveChanges();
            });
        }

        protected void AtomicDbAction(Action<PostsCommentsContext> dbUnitOfWorkAction)
        {
            using (var db = new PostsCommentsContext(dbOptions))
            {
                dbUnitOfWorkAction(db);
            }
        }        
    }
}
