using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic
{
    public interface IService<T> where T : IDomainModel
    {
        Task AddAsync(T post);
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}