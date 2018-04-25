using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService
    {
        private readonly IRepository<Comment> repository;
        private readonly IRepository<Post> postRepository;
        private readonly CommentValidator validator;

        public CommentsService(IRepository<Comment> repository, IRepository<Post> postRepository)
        {
            this.repository = repository;
            this.postRepository = postRepository;
            validator = new CommentValidator();
        }

        public Comment Get(Guid id)
        {
            return repository.Get(new ByIdSpecification<Comment>(id));
        }

        public IEnumerable<Comment> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            return repository.GetAllFiltered(new CommentsByPostSpecification(postId));
        }

        public IEnumerable<Comment> GetAllReadByPost(Guid postId)
        {
            return repository.GetAllFiltered(new CommentsByPostSpecification(postId).And(new OnlyIsReadCommentSpecification()));
        }

        public bool Add(Comment comment)
        {
            validator.Validate(comment);

            var postAlreadyAdded = postRepository.Get(new ByIdSpecification<Post>(comment.PostId));

            if (postAlreadyAdded == null)
                throw new Exception("post not found for adding comment");

            var commentAlreadyAdded = Get(comment.Id);

            if (commentAlreadyAdded != null)
                throw new Exception("comment not found for adding comment");

            return repository.Add(comment);
        }

        public bool Delete(Guid id)
        {
            var comment = Get(id);

            if (comment == null)
                throw new Exception("comment not found for delete");

            return repository.Delete(comment);
        }
    }
}
