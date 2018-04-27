using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic
{
    public interface IService<T> where T : IDomainModel
    {
        void Add(T post);
        T Get(Guid id);
        IEnumerable<T> GetAll();
        void Delete(Guid id);
    }
}