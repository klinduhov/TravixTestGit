using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface ICommentsRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetAllByPost(Guid postId);
    }
}
