using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface ICommentsRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId);
    }
}
