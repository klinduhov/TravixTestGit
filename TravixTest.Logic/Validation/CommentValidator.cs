using System;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Validation
{
    public class CommentValidator : ModelValidatorBase<Comment, CommentValidationException>
    {
        public CommentValidator()
        {
            AddRule(c => c.Id != Guid.Empty, () => new CommentValidationException("Cannot be empty", CommentValidatedAttribute.Id));
            AddRule(c => c.PostId != Guid.Empty, () => new CommentValidationException("Cannot be empty", CommentValidatedAttribute.Id));
            AddRule(c => !string.IsNullOrWhiteSpace(c.Text), () => new CommentValidationException("Cannot be empty", CommentValidatedAttribute.Text));
        }
    }
}