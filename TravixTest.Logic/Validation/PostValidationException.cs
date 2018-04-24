using System;

namespace TravixTest.Logic.Validation
{
    public class PostValidationException : Exception
    {
        public PostValidatedAttribute InvalidAttribute { get; }

        public PostValidationException(string message, PostValidatedAttribute invalidAttribute) : base(message)
        {
            InvalidAttribute = invalidAttribute;
        }
    }
}