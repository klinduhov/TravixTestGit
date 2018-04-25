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
            var leftExpression = left.IsSatisifiedBy();
            var rightExpression = right.IsSatisifiedBy();

            return Compose(leftExpression, rightExpression, Expression.AndAlso);
        }

        private Expression<TExpr> Compose<TExpr>(Expression<TExpr> first, Expression<TExpr> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, index) => new { f, s = second.Parameters[index] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<TExpr>(merge(first.Body, secondBody), first.Parameters);
        }
    }
}
