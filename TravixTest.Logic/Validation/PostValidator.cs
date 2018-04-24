using System;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Validation
{
    public class PostValidator : ModelValidatorBase<Post, PostValidationException>
    {
        public PostValidator()
        {
            AddRule(p => p.Id != Guid.Empty, () => new PostValidationException("Cannot be empty", PostValidatedAttribute.Id));
            AddRule(p => !string.IsNullOrWhiteSpace(p.Body), () => new PostValidationException("Cannot be empty", PostValidatedAttribute.Body));
        }
    }
}