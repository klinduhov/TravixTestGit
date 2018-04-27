using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostsService
    {
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task<Post> GetAsync(Guid id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}