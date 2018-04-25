using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public class ByIdSpecification<T> : SpecificationBase<T> where T : IDomainModel
    {
        private readonly Guid id;

        public ByIdSpecification(Guid id)
        {
            this.id = id;
        }

        public override Expression<Func<T, bool>> IsSatisifiedBy()
        {
            return m => m.Id == id;
        }
    }
}