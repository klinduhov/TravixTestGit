using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public class OnlyIsReadCommentSpecification : SpecificationBase<Comment>
    {
        public override Expression<Func<Comment, bool>> IsSatisifiedBy()
        {
            // "=true" remained to make the expression more readable
            return c => c.IsRead == true;
        }
    }
}