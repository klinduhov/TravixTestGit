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
}
