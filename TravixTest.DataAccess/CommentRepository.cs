using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{
    public class CommentRepository : RepositoryBase<Comment, CommentEntity>
    {
        protected CommentRepository(DbContextOptions<PostsCommentsContext> dbOptions) : base(dbOptions)
        {
        }

        public override void Delete(Comment model)
        {
            Delete(model, c => new CommentEntity { Id = c.Id });
        }

        public override IEnumerable<Comment> GetAll()
        {
            var result = new List<CommentEntity>();

            AtomicDbAction(db =>
            {
                result = db.Comments.ToList();
            });

            return result.Select(MapEntityToModel);
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
