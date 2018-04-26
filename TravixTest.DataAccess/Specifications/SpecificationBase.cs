using System;
using System.Linq.Expressions;
using TravixTest.DataAccess.Entities;

namespace TravixTest.DataAccess.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T> where T : IEntity
    {
        public SpecificationBase<T> And(SpecificationBase<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public abstract Expression<Func<T, bool>> IsSatisifiedBy();
    }
}