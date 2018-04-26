using System;
using System.Linq.Expressions;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess.Specifications
{
    public class CommentsByPostSpecification : SpecificationBase<CommentEntity>
    {
        private readonly Guid postId;

        public CommentsByPostSpecification(Guid postId)
        {
            this.postId = postId;
        }

        public override Expression<Func<CommentEntity, bool>> IsSatisifiedBy()
        {
            return c => c.PostId == postId;
        }
    }
}