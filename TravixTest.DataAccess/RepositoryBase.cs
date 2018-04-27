using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var entity = withIdEntityConstructor(model);
            AtomicModify((db, e) => 
            {
                db.Attach(e);
                db.Remove(e);
            }, entity);
        }

        public abstract IEnumerable<TModel> GetAll();
        public abstract void Delete(TModel model);
        //public abstract void Add(TModel model);

        public void Add(TModel model)
        {
            TEntity entity = MapModelToEntity(model);
            AtomicModify((db, e) => db.Add(e), entity);
        }

        //public void Delete(TModel model)
        //{
        //    //AtomicModify()
        //    throw new NotImplementedException();
        //}

        public TModel Get(Guid id)
        {
            TEntity result = null;
            
            AtomicDbAction(db =>
            {
                result = db.Set<TEntity>().SingleOrDefault(en => en.Id == id);
            });

            return MapEntityToModel(result);
        }

        //public IEnumerable<TModel> GetAll()
        //{
        //    var result = new List<TEntity>();

        //    AtomicDbAction(db =>
        //    {
        //        result = db.Set<TEntity>().ToList();
        //    });

        //    return result.Select(MapEntityToModel);
        //}

        //public void Update(TModel model)
        //{
        //    throw new NotImplementedException();
        //}

        //private TEntity GetScalarSingleAtomic<TEntity>(Func<PostsCommentsContext, IQueryable<TEntity>> getQueryForScalarFunc) where TEntity : class
        //{
        //    TEntity result = null;

        //    AtomicDbAction(db =>
        //    {
        //        result = getQueryForScalarFunc(db).Single();
        //    });

        //    return result;
        //}

        protected void AtomicModify(Action<PostsCommentsContext, TEntity> dbUnitOfWorkSaveAction, TEntity entityForSave)
        {
            AtomicDbAction(db =>
            {
                dbUnitOfWorkSaveAction(db, entityForSave);
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
