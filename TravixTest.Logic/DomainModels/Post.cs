using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TravixTest.Logic.DomainModels
{
    public class Post
    {
        public Guid Id { get; }
        public string Body { get; }
        public ICollection<Comment> Comments { get; } = new Collection<Comment>();

        public Post(Guid id, string body)
        {
            Id = id;
            Body = body;
        }

        public Post(Guid id, string body, ICollection<Comment> comments)
        {
            Id = id;
            Body = body;
            Comments = comments;
        }
    }
}
