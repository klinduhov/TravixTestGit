using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{

    public class PostsRepository : RepositoryBase<Post, PostEntity>, IPostsRepository
    {
        public PostsRepository(DbContextOptions<PostsCommentsContext> dbOptions) : base(dbOptions)
        {
        }

        public override async Task<Post> GetAsync(Guid id)
        {
            PostEntity result;

            using (var db = GenerateContext())
            {
                result = await db.Posts.Include(p => p.Comments).SingleOrDefaultAsync(en => en.Id == id);
            }

            return MapEntityToModel(result);
        }

        public override async Task<IEnumerable<Post>> GetAllASync()
        {
            using (var db = GenerateContext())
            {
                var result = await db.Posts.Include(p => p.Comments).ToListAsync();
                return result.Select(MapEntityToModel);
            }
        }

        public override async Task DeleteAsync(Post model)
        {
            await DeleteAsync(model, p => new PostEntity { Id = p.Id });
        }

        public async Task UpdateAsync(Post model)
        {
            using (var db = GenerateContext())
            {
                var entity = new PostEntity { Id = model.Id };
                db.Set<PostEntity>().Attach(entity);
                entity.Body = model.Body;

                await db.SaveChangesAsync();
            }
        }

        protected override Post MapEntityToModel(PostEntity entity)
        {
            return entity.Map();
        }

        protected override PostEntity MapModelToEntity(Post model)
        {
            return model.Map();
        }
    }
}
