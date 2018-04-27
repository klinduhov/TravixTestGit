using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId);
        Task AddAsync(Comment comment);
        Task<Comment> GetAsync(Guid id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}