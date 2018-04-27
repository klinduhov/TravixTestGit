using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IRepository<T> where T: IDomainModel
    {
        T Get(Guid id);
        IEnumerable<T> GetAll();
        void Add(T model);
        void Delete(T model);
    }
}
