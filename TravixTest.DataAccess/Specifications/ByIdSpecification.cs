using System;
using System.Linq.Expressions;
using TravixTest.DataAccess.Entities;

namespace TravixTest.DataAccess.Specifications
{
    public class ByIdSpecification<TEntity> : SpecificationBase<TEntity>
        where TEntity : IEntity
    {
        private readonly Guid id;

        public ByIdSpecification(Guid id)
        {
            this.id = id;
        }

        public override Expression<Func<TEntity, bool>> IsSatisifiedBy()
        {
            return e => e.Id == id;
        }
    }
}