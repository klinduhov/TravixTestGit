using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public class CommentsByPostSpecification : SpecificationBase<Comment>
    {
        private readonly Guid postId;

        public CommentsByPostSpecification(Guid postId)
        {
            this.postId = postId;
        }

        public override Expression<Func<Comment, bool>> IsSatisifiedBy()
        {
            return c => c.PostId == postId;
        }
    }
}