using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic
{
    public class PostsService
    {
        private readonly IPostRepository repository;

        public PostsService(IPostRepository repository)
        {
            this.repository = repository;
        }

        public Post Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return repository.GetAll();
        }

        public void Add(Post post)
        {
            Validate(post);

            var postAlreadyAdded = Get(post.Id);

            if (postAlreadyAdded != null)
                throw new Exception("post already added");

            repository.Add(post);
        }

        public void Update(Post post)
        {
            Validate(post);

            var oldPost = Get(post.Id);

            if (oldPost == null)
                throw new Exception("post not found for update");

            repository.Add(post);
        }

        public void Delete(Guid id)
        {
            var post = Get(id);

            if (post == null)
                throw new Exception("post not found for delete");

            repository.Delete(id);
        }

        public void Validate(Post post)
        {
            if (post.Id == Guid.Empty)
                throw new PostValidationException("Cannot be empty", PostValidatedAttribute.Id);

            if (string.IsNullOrWhiteSpace(post.Body))
                throw new PostValidationException("Cannot be empty", PostValidatedAttribute.Body);
        }
    }

    public class PostValidationException : Exception
    {
        public PostValidatedAttribute InvalidAttribute { get; }

        public PostValidationException(string message, PostValidatedAttribute invalidAttribute) : base(message)
        {
            InvalidAttribute = invalidAttribute;
        }
    }

    public enum PostValidatedAttribute
    {
        Id,
        Body
    }
}
