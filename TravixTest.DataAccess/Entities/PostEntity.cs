using System;
using System.Collections.Generic;
using System.Text;

namespace TravixTest.DataAccess.Entities
{
    public class PostEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public ICollection<CommentEntity> Comments { get; set; }
    }
}
