using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.WebApi.Models;

namespace TravixTest.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Posts")]
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICommentsService commentsService;

        // GET: api/Posts
        public PostsController(IPostsService postsService, ICommentsService commentsService)
        {
            this.postsService = postsService;
            this.commentsService = commentsService;
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
            postsService.Add(post);
        }

        // PUT: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]PostInputModel postModel)
        {
            var post = new Post(id, postModel.Body);
            postsService.Update(post);
        }

        // DELETE: api/Posts/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            postsService.Delete(id);
        }
    }
}
