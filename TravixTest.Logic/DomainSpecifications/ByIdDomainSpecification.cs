using System;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.DomainSpecifications
{
    public class ByIdDomainSpecification : DomainSpecificationBase
    {
        public Guid Id { get; }

        public ByIdDomainSpecification(Guid id) : base(Filters.ById)
        {
            Id = id;
        }        
    }

    //public class PostByIdDomainSpecification : ByIdDomainSpecification
    //{
    //    public PostByIdDomainSpecification(Guid id) : base(id)
    //    {
    //    }
    //}

    //public class CommentByIdDomainSpecification : ByIdDomainSpecification
    //{
    //    protected CommentByIdDomainSpecification(Guid id) : base(id)
    //    {
    //    }
    //}
}
