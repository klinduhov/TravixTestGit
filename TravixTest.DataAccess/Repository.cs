using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravixTest.DataAccess.Entities;
using TravixTest.DataAccess.Specifications;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.DomainSpecifications;
using TravixTest.Logic.Specifications;

namespace TravixTest.DataAccess
{
    internal class EntityRepository<TEntity> where TEntity : class
    {
        private readonly DbContextOptions<PostsCommentsContext> dbOptions;

        public EntityRepository(DbContextOptions<PostsCommentsContext> dbOptions)
        {
            this.dbOptions = dbOptions;
        }


    }

    public class Repository<TModel, TEntity> : IRepository<TModel> 
        where TModel : IDomainModel
        where TEntity : class, IEntity
    {
        private readonly DbContextOptions<PostsCommentsContext> dbOptions;

        public Repository(DbContextOptions<PostsCommentsContext> dbOptions)
        {
            this.dbOptions = dbOptions;
        }

        public bool Add(TModel model)
        {
            AtomicDbAction(db =>
            {

            });
        }

        public bool Delete(TModel model)
        {
            throw new NotImplementedException();
        }

        public TModel Get(IEnumerable<DomainSpecificationBase> specifications)
        {
            using (var db = new PostsCommentsContext(dbOptions))
            {
                var specification = SpecificationsListMapper.MapList<TEntity>(specifications);
                var entity = db.Set<TEntity>().Single(specification.IsSatisifiedBy().Compile());
            }
        }

        public IEnumerable<TModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> GetAllFiltered(IEnumerable<DomainSpecificationBase> specifications)
        {
            throw new NotImplementedException();
        }

        public bool Update(TModel model)
        {
            throw new NotImplementedException();
        }

        //private IEnumerable<TEntity> GetVectorAtomic<TEntity>(Func<PostsCommentsContext, TEntity> dbUnitOfWorkVectorFunc) where TEntity : class
        //{
        //    TEntity result = null;

        //    AtomicDbAction(db =>
        //    {
        //        result = dbUnitOfWorkScalarFunc(db);
        //    });

        //    return result;
        //}

        //private TEntity GetScalarSingleAtomic<TEntity>(Func<PostsCommentsContext, IQueryable<TEntity>> getQueryForScalarFunc, ISpecification) where TEntity : class
        //{
        //    TEntity result = null;

        //    AtomicDbAction(db =>
        //    {
        //        result = getQueryForScalarFunc(db).Single();
        //    });

        //    return result;
        //}

        //private void AtomicSave<TEntity>(Action<PostsCommentsContext, TEntity> dbUnitOfWorkSaveAction, TEntity entityForSave)
        //{
        //    AtomicDbAction(db => 
        //    {
        //        dbUnitOfWorkSaveAction(db, entityForSave);
        //        db.SaveChanges();
        //    });
        //}

        //private void AtomicDbAction(Action<PostsCommentsContext> dbUnitOfWorkAction)
        //{
        //    using (var db = new PostsCommentsContext(dbOptions))
        //    {
        //        dbUnitOfWorkAction(db);
        //    }
        //}
    }
}
