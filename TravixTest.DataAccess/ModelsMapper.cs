using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravixTest.DataAccess.Entities;
using TravixTest.Logic.DomainModels;

namespace TravixTest.DataAccess
{
    static class ModelsMapper
    {
        //public static PostEntity Map(this Post model)
        //public static IEntity MapToEntity<TModel>(this TModel model)
        //    where TModel : IDomainModel
        //{
        //    var post = model as Post;

        //    if (post != null)
        //        return post.Map();

        //    var comment = model as Comment;

        //    if (comment != null)
        //        return comment.Map();

        //    throw new ArgumentOutOfRangeException();
        //}

        //public static TModel MapToModel<TEntity, TModel>(this TEntity entity)
        //    where TEntity : IEntity
        //{
        //    var post = entity as PostEntity;

        //    if (post != null)
        //        return post.Map();

        //    var comment = entity as CommentEntity;

        //    if (comment != null)
        //        return comment.Map();

        //    throw new ArgumentOutOfRangeException();
        //}

        public static PostEntity Map(this Post model)
        {
            return new PostEntity { Id = model.Id, Body = model.Body };
        }

        public static CommentEntity Map(this Comment model)
        {
            return new CommentEntity { Id = model.Id, Text = model.Text };
        }

        public static Post Map(this PostEntity entity)
        {
            return new Post(entity.Id, entity.Body, entity.Comments.Select(c => c.Map()).ToList());
        }

        public static Comment Map(this CommentEntity entity)
        {
            return new Comment(entity.Id, entity.PostId, entity.Text);
        }
    }
}
