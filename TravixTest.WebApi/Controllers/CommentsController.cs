using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;
using TravixTest.WebApi.Models;

namespace TravixTest.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService service;
        private readonly ILogger<CommentsController> logger;

        // GET: api/Posts
        public CommentsController(ICommentsService service, ILogger<CommentsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<IEnumerable<Comment>> Get()
        {
            return await service.GetAllAsync();
        }

        // GET: api/Comments/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpGet("{id}")]
        public async Task<Comment> Get(Guid id)
        {
            return await service.GetAsync(id);
        }

        // POST: api/Comments
        [HttpPost]
        public async Task Post([FromBody]CommentInputModel commentInput)
        {
            try
            {
                await service.AddAsync(new Comment(Guid.NewGuid(), commentInput.PostId, commentInput.Text));
            }
            catch (CommentValidationException e)
            {
                logger.LogError($"CommentValidationException <{e.Message}> {e.InvalidAttribute}");
                throw;
            }
        }

        // DELETE: api/Comments/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await service.DeleteAsync(id);
        }
    }
}
