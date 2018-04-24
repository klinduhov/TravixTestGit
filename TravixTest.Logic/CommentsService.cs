using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic
{    
    public class CommentsService
    {
        private readonly ICommentRepository repository;
        private readonly IPostRepository postRepository;

        public CommentsService(ICommentRepository repository, IPostRepository postRepository)
        {
            this.repository = repository;
            this.postRepository = postRepository;
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
            Validate(comment);

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

        private void Validate(Comment comment)
        {
            if (comment.Id == Guid.Empty)
                throw new CommentValidationException("Cannot be empty", CommentValidatedAttribute.Id);

            if (comment.PostId == Guid.Empty)
                throw new CommentValidationException("Cannot be empty", CommentValidatedAttribute.PostId);

            if (string.IsNullOrWhiteSpace(comment.Text))
                throw new CommentValidationException("Cannot be empty", CommentValidatedAttribute.Text);
        }
    }

    public class CommentValidationException : Exception
    {
        public CommentValidatedAttribute InvalidAttribute { get; }

        public CommentValidationException(string message, CommentValidatedAttribute invalidAttribute) : base(message)
        {
            InvalidAttribute = invalidAttribute;
        }
    }

    public enum CommentValidatedAttribute
    {
        Id,
        PostId,
        Text
    }
}
