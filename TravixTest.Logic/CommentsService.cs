using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService : ServiceBase<Comment, CommentValidationException>, IService<Comment>, ICommentsService
    {
        private readonly ICommentsRepository repository;
        private readonly IRepository<Post> postRepository;

        public CommentsService(ICommentsRepository repository, IPostsRepository postRepository) :
            base(repository, new CommentValidator())
        {
            this.repository = repository;
            this.postRepository = postRepository;
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            return repository.GetAllByPost(postId);
        }

        public void Add(Comment comment)
        {
            Add(comment, c =>
            {
                var postAlreadyAdded = postRepository.Get(c.PostId);

                if (postAlreadyAdded == null)
                    throw new Exception("post not found for adding comment");

                var commentAlreadyAdded = Get(c.Id);

                if (commentAlreadyAdded != null)
                    throw new Exception("already added");
            });
        }
    }
}
