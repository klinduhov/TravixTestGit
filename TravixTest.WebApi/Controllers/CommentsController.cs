using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.WebApi.Models;

namespace TravixTest.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService service;

        // GET: api/Posts
        public CommentsController(ICommentsService service)
        {
            this.service = service;
        }

        // GET: api/Comments
        [HttpGet]
        public IEnumerable<Comment> Get()
        {
            return service.GetAll();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public Comment Get(Guid id)
        {
            return service.Get(id);
        }
        
        // POST: api/Comments
        [HttpPost]
        public void Post([FromBody]CommentInputModel commentInput)
        {
            service.Add(new Comment(Guid.NewGuid(), commentInput.PostId, commentInput.Text));
        }
                
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            service.Delete(id);
        }
    }
}
