using System;
using System.Collections.Generic;
using System.Linq;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic;
using TravixTest.Logic.DomainSpecifications;

namespace TravixTest.DataAccess.Specifications
{
    public static class Extension
    {
        public static SpecificationBase<CommentEntity> Map(this CommentsByPostDomainSpecification specification)
        {
            return new CommentsByPostSpecification(specification.PostId);
        }
    }

    public static class SpecificationsListMapper
    {
        private readonly IDictionary<Filters, Func> specificationConstructorDictionary = new Dictionary<Filters, T>
        {
            {Filters.ById, (id) => new ByIdSpecification<T>(id) }
        }
        public static SpecificationBase<T> MapList<T>(IEnumerable<DomainSpecificationBase> domainSpecificationsList) where T : IEntity =>
            domainSpecificationsList.Select(ds => Map<T>(ds)).Aggregate((f, s) => f.And(s));

        private static SpecificationBase<T> Map<T>(DomainSpecificationBase domainSpecification) where T : IEntity
        {
            switch (domainSpecification.Filter)
            {
                case Filters.ById:
                    return new ByIdSpecification<T>(((ByIdDomainSpecification)domainSpecification).Id);
                case Filters.CommentByPostId:
                    return  new CommentsByPostSpecification(((CommentsByPostDomainSpecification)domainSpecification).PostId);
                case Filters.CommentIsReadOnly:
                    return new OnlyIsReadCommentSpecification();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
