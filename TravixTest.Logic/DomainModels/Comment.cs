using System;

namespace TravixTest.Logic.DomainModels
{
    public class Comment : IDomainModel
    {        
        public Guid Id { get; }
        public Guid PostId { get; }
        public string Text { get; }
        public DateTime CreateDateTimeUtc { get; }

        public Comment(Guid id, Guid postId, string text)
        {
            Id = id;
            Text = text;
            PostId = postId;
            CreateDateTimeUtc = DateTime.UtcNow;
        }
    }
}