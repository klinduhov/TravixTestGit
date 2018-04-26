using System;

namespace TravixTest.Logic.DomainSpecifications
{
    public class CommentsByPostDomainSpecification : DomainSpecificationBase
    {
        public Guid PostId { get; }

        public CommentsByPostDomainSpecification(Guid postId) : base(Filters.CommentByPostId)
        {
            PostId = postId;
        }        
    }
}
