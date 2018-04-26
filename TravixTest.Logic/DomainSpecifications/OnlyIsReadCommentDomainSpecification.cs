namespace TravixTest.Logic.DomainSpecifications
{
    public class OnlyIsReadCommentDomainSpecification : DomainSpecificationBase
    {
        public OnlyIsReadCommentDomainSpecification() : base(Filters.CommentIsReadOnly)
        {
        }
    }
}
