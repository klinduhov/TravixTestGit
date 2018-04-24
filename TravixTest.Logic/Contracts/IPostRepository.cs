using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

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

    public interface IQueryProvider<T> where T : IDomainModel
    {
        T ApplyScalarFilter();
        IEnumerable<T> ApplyVectorFilter();
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
