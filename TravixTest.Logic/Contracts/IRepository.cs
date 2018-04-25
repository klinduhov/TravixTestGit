using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;

namespace TravixTest.Logic.Contracts
{
    public interface IRepository<T> where T: IDomainModel
    {
        T Get(ISpecification<T> specification);
        IEnumerable<T> GetAllFiltered(ISpecification<T> specification);
        IEnumerable<T> GetAll();
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
    }
}
