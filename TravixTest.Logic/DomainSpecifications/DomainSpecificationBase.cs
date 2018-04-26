namespace TravixTest.Logic.DomainSpecifications
{
    public abstract class DomainSpecificationBase
    {
        public Filters Filter { get; }

        protected DomainSpecificationBase(Filters filter)
        {
            Filter = filter;
        }        
    }
}
