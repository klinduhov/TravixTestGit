using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    class PostAllCommentsSpecification : ISpecification<Comment>
    {
        private readonly Guid postId;

        public PostAllCommentsSpecification(Guid postId)
        {
            this.postId = postId;
        }

        public Expression<Func<Comment, bool>> IsSatisifiedBy()
        {
            return c => c.PostId == postId;
        }
    }

    class PostCreatedLessThanDayAgoCommentsSpecification : ISpecification<Comment>
    {
        private readonly Guid postId;

        public PostCreatedLessThanDayAgoCommentsSpecification(Guid postId)
        {
            this.postId = postId;
        }

        public Expression<Func<Comment, bool>> IsSatisifiedBy()
        {
            return c => c.PostId == postId && c.CreateDateTimeUtc > DateTime.UtcNow.AddDays(-1);
        }
    }
}