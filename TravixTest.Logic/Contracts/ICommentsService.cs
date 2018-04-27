using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface ICommentsService
    {
        IEnumerable<Comment> GetAllByPost(Guid postId);
        void Add(Comment comment);
        Comment Get(Guid id);
        IEnumerable<Comment> GetAll();
        void Delete(Guid id);
    }
}