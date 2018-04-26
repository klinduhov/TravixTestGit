using Microsoft.EntityFrameworkCore;
using TravixTest.DataAccess.Entities;

namespace TravixTest.DataAccess
{
    public class PostsCommentsContext : DbContext
    {
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        public PostsCommentsContext(DbContextOptions<PostsCommentsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<PostEntity>().Property(p => p.Body).IsRequired();
            modelBuilder.Entity<PostEntity>().HasMany(p => p.Comments).WithOne().HasForeignKey(c => c.PostId);//.WithOne(c => c.Post);

            modelBuilder.Entity<CommentEntity>().HasKey(c => c.Id);
            modelBuilder.Entity<CommentEntity>().Property(p => p.Text).IsRequired();
            modelBuilder.Entity<CommentEntity>().Property(c => c.PostId).IsRequired();
        }
    }
}
