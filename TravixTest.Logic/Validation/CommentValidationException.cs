using System;

namespace TravixTest.Logic.Validation
{
    public class CommentValidationException : Exception
    {
        public CommentValidatedAttribute InvalidAttribute { get; }

        public CommentValidationException(string message, CommentValidatedAttribute invalidAttribute) : base(message)
        {
            InvalidAttribute = invalidAttribute;
        }
    }
}
