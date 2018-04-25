using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T> where T : IDomainModel
    {
        public SpecificationBase<T> And(SpecificationBase<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public abstract Expression<Func<T, bool>> IsSatisifiedBy();
    }
}