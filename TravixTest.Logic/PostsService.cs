using System;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class PostsService : ServiceBase<Post, PostValidationException>, IService<Post>
    {
        private readonly IPostRepository repository;

        public PostsService(IPostRepository repository) : base(repository, new PostValidator())
        {
            this.repository = repository;
        }

        public void Add(Post post)
        {
            Add(post, p =>
            {
                var postAlreadyAdded = Get(p.Id);

                if (postAlreadyAdded != null)
                    throw new Exception("already added");
            });
        }

        public void Update(Post post)
        {
            Validator.Validate(post);

            var oldPost = Get(post.Id);

            if (oldPost == null)
                throw new Exception("not found for update");

            repository.Update(post);
        }
    }
}
