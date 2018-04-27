using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;
using TravixTest.WebApi.Models;

namespace TravixTest.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Posts")]
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICommentsService commentsService;
        private readonly ILogger<PostsController> logger;

        // GET: api/Posts
        public PostsController(IPostsService postsService, ICommentsService commentsService, ILogger<PostsController> logger)
        {
            this.postsService = postsService;
            this.commentsService = commentsService;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return postsService.GetAll();
        }

        // GET: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpGet("{id}")]
        public Post Get(Guid id)
        {
            return postsService.Get(id);
        }

        // GET: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7/comments
        [Route("{id}/comments")]
        [HttpGet]
        public IEnumerable<Comment> GetComments(Guid id)
        {
            return commentsService.GetAllByPost(id);
        }

        // POST: api/Posts
        [HttpPost]
        public void Post([FromBody]PostInputModel postInput)
        {
            var post = new Post(Guid.NewGuid(), postInput.Body);

            try
            {
                postsService.Add(post);
            }
            catch (PostValidationException e)
            {
                logger.LogError($"PostValidationException <{e.Message}> {e.InvalidAttribute}");
                throw;
            }
            
        }

        // PUT: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]PostInputModel postModel)
        {
            var post = new Post(id, postModel.Body);

            try
            {
                postsService.Update(post);
            }
            catch (PostValidationException e)
            {
                logger.LogError($"PostValidationException <{e.Message}> {e.InvalidAttribute}");
                throw;
            }
            
        }

        // DELETE: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            postsService.Delete(id);
        }
    }
}
