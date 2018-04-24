using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService
    {
        private readonly ICommentRepository repository;
        private readonly IPostRepository postRepository;
        private readonly CommentValidator validator;

        public CommentsService(ICommentRepository repository, IPostRepository postRepository)
        {
            this.repository = repository;
            this.postRepository = postRepository;
            validator = new CommentValidator();
        }

        public Comment Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            return repository.GetAllByPost(postId);
        }

        public bool Add(Comment comment)
        {
            validator.Validate(comment);

            var postAlreadyAdded = postRepository.Get(comment.PostId);

            if (postAlreadyAdded == null)
                throw new Exception("post not found for adding comment");

            var commentAlreadyAdded = repository.Get(comment.Id);

            if (commentAlreadyAdded != null)
                throw new Exception("comment not found for adding comment");

            return repository.Add(comment);
        }

        public bool Delete(Guid id)
        {
            var comment = Get(id);

            if (comment == null)
                throw new Exception("comment not found for delete");

            return repository.Delete(id);
        }
    }
}
