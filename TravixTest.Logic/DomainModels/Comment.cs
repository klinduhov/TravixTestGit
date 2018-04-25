using System;

namespace TravixTest.Logic.DomainModels
{
    public class Comment : IDomainModel
    {        
        public Guid Id { get; }
        public Guid PostId { get; }
        public string Text { get; }
        public bool IsRead { get; set; }

        public Comment(Guid id, Guid postId, string text, bool isRead = false)
        {
            Id = id;
            Text = text;            
            PostId = postId;
            IsRead = isRead;
        }
    }
}