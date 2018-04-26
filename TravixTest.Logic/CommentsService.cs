using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.DomainSpecifications;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService : ServiceBase<Comment, CommentValidationException>, IService<Comment>
    {
        private readonly IRepository<Post> postRepository;

        public CommentsService(IRepository<Comment> repository, IRepository<Post> postRepository) :
            base(repository, new CommentValidator())
        {
            this.postRepository = postRepository;
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            return Repository.GetAllFiltered(new Collection<DomainSpecificationBase> { new CommentsByPostDomainSpecification(postId) });
        }

        public IEnumerable<Comment> GetAllReadByPost(Guid postId)
        {
            return Repository.GetAllFiltered(new Collection<DomainSpecificationBase>
            {
                new CommentsByPostDomainSpecification(postId),
                new OnlyIsReadCommentDomainSpecification()
            });
        }

        public bool Add(Comment comment)
        {
            return Add(comment, c =>
            {
                var postAlreadyAdded = postRepository.Get(new Collection<DomainSpecificationBase> { new ByIdDomainSpecification(c.PostId) });

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
