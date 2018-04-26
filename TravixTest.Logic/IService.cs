using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic
{
    public interface IService<T> where T : IDomainModel
    {
        bool Add(T post);
        T Get(Guid id);
        IEnumerable<T> GetAll();
        bool Delete(Guid id);
    }
}