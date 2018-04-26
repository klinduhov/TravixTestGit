using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TravixTest.DataAccess.Entities
{
    public class CommentEntity : IEntity
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public PostEntity Post { get; set; }
    }
}
