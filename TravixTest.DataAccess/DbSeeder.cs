using System;
using System.Collections.Generic;
using System.Linq;
using TravixTest.DataAccess.Entities;

namespace TravixTest.DataAccess
{
    public class DbSeeder
    {
        public static void Seed(PostsCommentsContext context)
        {
            context.Database.EnsureCreated();

            if (context.Posts.Any())
                return;

            var comments = new List<CommentEntity>();
            var posts = new List<PostEntity>();

            posts.AddRange(Enumerable.Range(0, 2).Select(i =>
            {
                var postId = Guid.NewGuid();

                comments.AddRange(Enumerable.Range(0, 2)
                    .Select(j => new CommentEntity
                    {
                        Id = Guid.NewGuid(),
                        PostId = postId,
                        IsRead = false,
                        Text = $"comment {j} for post {i}"
                    }));

                return new PostEntity {Id = postId, Body = $"test body {i}"};
            }));

            context.Posts.AddRange(posts);
            context.Comments.AddRange(comments);

            context.SaveChanges();
        }
    }
}
