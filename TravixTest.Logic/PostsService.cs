using System;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class PostsService : ServiceBase<Post, PostValidationException>, IService<Post>
    {
        public PostsService(IRepository<Post> repository) : base(repository, new PostValidator())
        {
        }

        public bool Add(Post post)
        {
            return Add(post, p =>
            {
                var postAlreadyAdded = Get(p.Id);

                if (postAlreadyAdded != null)
                    throw new Exception("already added");
            });
        }

        public bool Update(Post post)
        {
            return Update(post, null);
        }
    }
}
