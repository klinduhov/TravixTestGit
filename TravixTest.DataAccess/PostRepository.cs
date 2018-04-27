using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{

    public class PostRepository : RepositoryBase<Post, PostEntity>
    {
        private readonly DbContextOptions<PostsCommentsContext> dbOptions;

        protected PostRepository(DbContextOptions<PostsCommentsContext> dbOptions) : base(dbOptions)
        {
            this.dbOptions = dbOptions;
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
            AtomicDbAction(db =>
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
