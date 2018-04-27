using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{
    static class ModelsEntitesMapper
    {
        public static PostEntity Map(this Post model)
        {
            return model == null ? null : new PostEntity { Id = model.Id, Body = model.Body };
        }

        public static CommentEntity Map(this Comment model)
        {
            return model == null ? null : new CommentEntity { Id = model.Id, PostId = model.PostId, Text = model.Text };
        }

        public static Post Map(this PostEntity entity)
        {
            return entity == null ? null : new Post(entity.Id, entity.Body, entity.Comments.Select(c => c.Map()).ToList());
        }

        public static Comment Map(this CommentEntity entity)
        {
            return entity == null ? null : new Comment(entity.Id, entity.PostId, entity.Text);
        }
    }
}
