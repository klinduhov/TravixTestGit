using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public IEnumerable<Comment> Get()
        {
            return service.GetAll();
        }

        // GET: api/Comments/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpGet("{id}")]
        public Comment Get(Guid id)
        {
            return service.Get(id);
        }

        // POST: api/Comments
        [HttpPost]
        public void Post([FromBody]CommentInputModel commentInput)
        {
            try
            {
                service.Add(new Comment(Guid.NewGuid(), commentInput.PostId, commentInput.Text));
            }
            catch (CommentValidationException e)
            {
                logger.LogError($"CommentValidationException <{e.Message}> {e.InvalidAttribute}");
                throw;
            }
        }

        // DELETE: api/Comments/C80D232D-9BB1-4BCD-9C7C-470FFD73D8A7
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            service.Delete(id);
        }
    }
}
