using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{
    public class CommentsService : ServiceBase<Comment, CommentValidationException>, IService<Comment>, ICommentsService
    {
        private readonly ICommentsRepository repository;
        private readonly IRepository<Post> postRepository;

        public CommentsService(ICommentsRepository repository, IPostsRepository postRepository) :
            base(repository, new CommentValidator())
        {
            this.repository = repository;
            this.postRepository = postRepository;
        }

        public async Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId)
        {
            return await repository.GetAllByPostAsync(postId);
        }

        public async Task AddAsync(Comment comment)
        {
            await AddAsync(comment, async c =>
             {
                 var postAlreadyAdded = await postRepository.GetAsync(c.PostId);

                 if (postAlreadyAdded == null)
                     throw new Exception("post not found for adding comment");

                 var commentAlreadyAdded = await GetAsync(c.Id);

                 if (commentAlreadyAdded != null)
                     throw new Exception("already added");
             });
        }
    }
}
