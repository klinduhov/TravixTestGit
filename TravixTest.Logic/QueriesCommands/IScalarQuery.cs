using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.QueriesCommands
{
    public interface IScalarQuery<out T> where T: IDomainModel
    {
        T Execute();
    }
}