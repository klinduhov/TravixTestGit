using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.QueriesCommands;

namespace TravixTest.Logic.Contracts
{
    public interface IPostRepository
    {
        Post Get(Guid id);
        IEnumerable<Post> GetAll();
        bool Add(Post post);
        bool Update(Post post);
        bool Delete(Guid id);
    }

    public interface IQueryFactory<T> where T : IDomainModel
    {
        IGetByIdQuery<T> GetByIdQuery();
        IEnumerable<T> GetAll(IQuery<T> query);
    }

    public interface IQueryProvider<T> where T : IDomainModel
    {
        T ApplyScalarFilter(IQuery<T> query);
        IEnumerable<T> ApplyVectorFilter(IQuery<T> query);
    }

    public abstract class ScalarQuery<T> where T : IDomainModel
    {
        private readonly IQueryProvider<T> queryProvider;

        protected ScalarQuery(IQueryProvider<T> queryProvider)
        {
            this.queryProvider = queryProvider;
        }
    }
}
