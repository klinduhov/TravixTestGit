using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public override Post Get(Guid id)
        {
            PostEntity result = null;

            AtomicDbAction(db =>
            {
                result = db.Posts.Include(p => p.Comments).SingleOrDefault(en => en.Id == id);
            });

            return MapEntityToModel(result);
        }

        public override IEnumerable<Post> GetAll()
        {
            var result = new List<PostEntity>();

            AtomicDbAction(db =>
            {
                result = db.Posts.Include(p => p.Comments).ToList();
            });

            return result.Select(MapEntityToModel);
        }

        public override void Delete(Post model)
        {
            Delete(model, p => new PostEntity { Id = p.Id });
        }

        public void Update(Post model)
        {
            AtomicModify(db =>
            {
                var entity = new PostEntity { Id = model.Id };
                db.Set<PostEntity>().Attach(entity);
                entity.Body = model.Body;
            });
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
