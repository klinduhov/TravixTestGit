﻿using System;
using System.Threading.Tasks;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class PostsService : ServiceBase<Post, PostValidationException>, IService<Post>, IPostsService
    {
        private readonly IPostsRepository repository;

        public PostsService(IPostsRepository repository) : base(repository, new PostValidator())
        {
            this.repository = repository;
        }

        public async Task AddAsync(Post post)
        {
            Validator.Validate(post);

            var postAlreadyAdded = await GetAsync(post.Id);

            if (postAlreadyAdded != null)
                throw new Exception("already added");

            await repository.AddAsync(post);
        }

        public async Task UpdateAsync(Post post)
        {
            Validator.Validate(post);

            var oldPost = await GetAsync(post.Id);

            if (oldPost == null)
                throw new Exception("not found for update");

            await repository.UpdateAsync(post);
        }
    }
}
