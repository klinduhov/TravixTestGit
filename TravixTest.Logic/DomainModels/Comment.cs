using System;

namespace TravixTest.Logic.DomainModels
{
    public class Comment
    {        
        public Guid Id { get; }
        public string Text { get; }

        public Comment(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}