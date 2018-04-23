﻿using System;
using System.Collections.Generic;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Contracts
{
    public interface IPostRepository
    {
        Post Get(Guid id);
        IEnumerable<Post> GetAll();
        void Add(Post post);
        void Delete(Guid id);
    }

    public interface ICommentRepository
    {
        Post Get(Guid id);
        IEnumerable<Post> GetAll();
        void Add(Post post);
        void Delete(Guid id);
    }
}