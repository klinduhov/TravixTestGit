using System;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Specifications
{
    public interface ISpecification<T> where T: IDomainModel
    {
        Expression<Func<T, bool>> IsSatisifiedBy();
    }
}
