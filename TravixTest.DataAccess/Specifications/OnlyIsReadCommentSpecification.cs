using System;
using System.Linq.Expressions;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess.Specifications
{
    public class OnlyIsReadCommentSpecification : SpecificationBase<CommentEntity>
    {
        public override Expression<Func<CommentEntity, bool>> IsSatisifiedBy()
        {
            // "=true" remained to make the expression more readable
            return c => c.IsRead == true;
        }
    }
}