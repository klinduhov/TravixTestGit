using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.DomainSpecifications;

namespace TravixTest.Logic.Contracts
{
    public interface IRepository<T> where T: IDomainModel
    {
        T Get(IEnumerable<DomainSpecificationBase> specifications);
        IEnumerable<T> GetAllFiltered(IEnumerable<DomainSpecificationBase> specifications);
        IEnumerable<T> GetAll();
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
    }
}
