using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.QueriesCommands
{
    public interface IQuery<T> where T : IDomainModel
    {
        IQuery<T> Add(IQuery<T> query);
    }

    public interface IGetByIdQuery<out T> : IScalarQuery<T> where T : IDomainModel
    {
    }
}