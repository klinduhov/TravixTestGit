using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface ICommentRepository
    {
        Comment Get(Guid id);
        IEnumerable<Comment> GetAll();
        IEnumerable<Comment> GetAllByPost(Guid postId);
        //IEnumerable<Comment> GetAllMatching();
        bool Add(Comment comment);
        bool Delete(Guid id);
    }
}