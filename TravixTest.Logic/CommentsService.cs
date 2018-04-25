using System;
using System.Collections.Generic;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService : ServiceBase<Comment, CommentValidationException>
    {
        private readonly IRepository<Post> postRepository;

        public CommentsService(IRepository<Comment> repository, IRepository<Post> postRepository) :
            base(repository, new CommentValidator())
        {
            this.postRepository = postRepository;
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            return Repository.GetAllFiltered(new CommentsByPostSpecification(postId));
        }

        public IEnumerable<Comment> GetAllReadByPost(Guid postId)
        {
            return Repository.GetAllFiltered(new CommentsByPostSpecification(postId).And(new OnlyIsReadCommentSpecification()));
        }

        public bool Add(Comment comment)
        {
            return Add(comment, c =>
            {
                var postAlreadyAdded = postRepository.Get(new ByIdSpecification<Post>(c.PostId));

                if (postAlreadyAdded == null)
                    throw new Exception("post not found for adding comment");

                var commentAlreadyAdded = Get(c.Id);

                if (commentAlreadyAdded != null)
                    throw new Exception("already added");
            });
        }

        public bool SetIsRead(Comment comment)
        {
            return Update(comment, c => c.IsRead = true);
        }
    }
}
