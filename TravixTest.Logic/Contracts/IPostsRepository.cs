using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostsRepository : IRepository<Post>
    {
        void Update(Post model);
    }
}
