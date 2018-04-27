using System;

namespace TravixTest.WebApi.Models
{
    public class PostInputModel
    {
        public string Body { get; set; }
    }

    public class CommentInputModel
    {
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
