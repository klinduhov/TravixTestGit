using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class PostsService : ServiceBase<Post, PostValidationException>
    {
        public PostsService(IRepository<Post> repository) : base(repository, new PostValidator())
        {
        }

        public bool Add(Post post)
        {
            return Add(post, p =>
            {
                var postAlreadyAdded = Get(post.Id);

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
