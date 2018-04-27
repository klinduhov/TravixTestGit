using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public override void Delete(Comment model)
        {
            Delete(model, c => new CommentEntity { Id = c.Id });
        }

        public override Comment Get(Guid id)
        {
            CommentEntity result = null;

            AtomicDbAction(db =>
            {
                result = db.Comments.SingleOrDefault(en => en.Id == id);
            });

            return MapEntityToModel(result);
        }

        public IEnumerable<Comment> GetAllByPost(Guid postId)
        {
            var result = new List<CommentEntity>();

            AtomicDbAction(db =>
            {
                result = db.Comments.Where(c => c.PostId == postId).ToList();
            });

            return result.Select(MapEntityToModel);
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
