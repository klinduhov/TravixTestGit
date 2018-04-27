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
    public class CommentsRepository : RepositoryBase<Comment, CommentEntity>, ICommentsRepository
    {
        public CommentsRepository(DbContextOptions<PostsCommentsContext> dbOptions) : base(dbOptions)
        {
        }

        public override async Task DeleteAsync(Comment model)
        {
            await DeleteAsync(model, c => new CommentEntity { Id = c.Id });
        }

        public override async Task<Comment> GetAsync(Guid id)
        {
            CommentEntity result;

            using (var db = GenerateContext())
            {
                result = await db.Comments.SingleOrDefaultAsync(en => en.Id == id);
            };

            return MapEntityToModel(result);
        }

        public async Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId)
        {
            List<CommentEntity> result;

            using (var db = GenerateContext())
            {
                result = await db.Comments.Where(c => c.PostId == postId).ToListAsync();
            }

            return result.Select(MapEntityToModel);
        }

        public override async Task<IEnumerable<Comment>> GetAllASync()
        {
            using (var db = GenerateContext())
            {
                var result = await db.Comments.ToListAsync();
                return result.Select(MapEntityToModel);
            }
        }

        protected override Comment MapEntityToModel(CommentEntity entity)
        {
            return entity.Map();
        }

        protected override CommentEntity MapModelToEntity(Comment model)
        {
            return model.Map();
        }
    }
}
