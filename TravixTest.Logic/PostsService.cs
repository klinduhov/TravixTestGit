using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class PostsService
    {
        private readonly IQueryFactory<Post> queryfactory;
        private readonly PostValidator validator;

        public PostsService(IQueryFactory<Post> queryfactory)
        {
            this.queryfactory = queryfactory;
            validator = new PostValidator();
        }

        public Post Get(Guid id)
        {
            return queryfactory.GetByIdQuery().Execute();
        }

        public IEnumerable<Post> GetAll()
        {
            return repository.GetAll();
        }

        public bool Add(Post post)
        {
            validator.Validate(post);

            var postAlreadyAdded = Get(post.Id);

            if (postAlreadyAdded != null)
                throw new Exception("post already added");

            return repository.Add(post);
        }

        public bool Update(Post post)
        {
            validator.Validate(post);

            var oldPost = Get(post.Id);

            if (oldPost == null)
                throw new Exception("post not found for update");

            return repository.Update(post);
        }

        public bool Delete(Guid id)
        {
            var post = Get(id);

            if (post == null)
                throw new Exception("post not found for delete");

            return repository.Delete(id);
        }
    }
}
