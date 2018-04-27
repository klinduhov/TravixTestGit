using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IRepository<T> where T: IDomainModel
    {
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllASync();
        Task AddAsync(T model);
        Task DeleteAsync(T model);
    }
}
