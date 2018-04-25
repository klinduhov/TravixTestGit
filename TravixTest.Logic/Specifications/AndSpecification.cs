using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public class AndSpecification<T> : SpecificationBase<T> where T : IDomainModel
    {
        private readonly ISpecification<T> left;
        private readonly ISpecification<T> right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override Expression<Func<T, bool>> IsSatisifiedBy()
        {
            BinaryExpression andExpression = Expression.AndAlso(left.IsSatisifiedBy().Body, right.IsSatisifiedBy().Body);

            return Expression.Lambda<Func<T, bool>>(andExpression, left.IsSatisifiedBy().Parameters.Single());
        }
    }
}
