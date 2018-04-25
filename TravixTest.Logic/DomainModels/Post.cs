using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TravixTest.Logic.DomainModels
{
    public class Post : IDomainModel
    {
        public Guid Id { get; }
        public string Body { get; }
        public ICollection<Comment> Comments { get; } = new Collection<Comment>();

        public Post(Guid id, string body)
        {
            Id = id;
            Body = body;
        }
    }
}
