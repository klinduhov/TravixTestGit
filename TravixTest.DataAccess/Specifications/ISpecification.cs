using System;
using System.Linq.Expressions;
using TravixTest.DataAccess.Entities;

namespace TravixTest.DataAccess.Specifications
{
    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification where T: IEntity
    {
        Expression<Func<T, bool>> IsSatisifiedBy();
    }
}
