using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.QueriesCommands
{
    public interface IVectorQuery<out T> where T : IDomainModel
    {
        IEnumerable<T> Execute();
    }
}
