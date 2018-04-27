using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostsService
    {
        void Add(Post post);
        void Update(Post post);
        Post Get(Guid id);
        IEnumerable<Post> GetAll();
        void Delete(Guid id);
    }
}