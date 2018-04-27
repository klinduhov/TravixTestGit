using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostRepository : IRepository<Post>
    {
        void Update(Post model);
    }
}
