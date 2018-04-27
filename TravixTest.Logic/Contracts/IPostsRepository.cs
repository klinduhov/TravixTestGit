using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostsRepository : IRepository<Post>
    {
        Task UpdateAsync(Post model);
    }
}
