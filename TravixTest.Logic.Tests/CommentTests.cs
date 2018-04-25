using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;
using TravixTest.Logic.Validation;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public class CommentTests
    {
        #region Facts



        #endregion

        #region Private methods

        private CommentsService CreateTestingService()
        {
            var commentsWereCreated = new List<Comment>();
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 2).Select(i => 
            {
                var postId = Guid.NewGuid();
                var comments = Enumerable.Range(0, 2).Select(j => new Comment(Guid.NewGuid(), postId, $"comment {j} for post {i}"));
                commentsWereCreated.AddRange(comments);

                return new Post(postId, $"test body {i}", comments.ToList());
            }));

            var mockCommentRepository = new Mock<IRepository<Comment>>();

            mockCommentRepository
                .Setup(r => r.GetAll())
                .Returns(() => commentsWereCreated);

            mockCommentRepository
                .Setup(r => r.Get(It.IsAny<ByIdSpecification<Comment>>()))
                .Returns<ByIdSpecification<Comment>>(sp => commentsWereCreated.SingleOrDefault(sp.IsSatisifiedBy().Compile()));

            var mockPostRepository = new Mock<IRepository<Post>>();

            mockPostRepository
                .Setup(r => r.Get(It.IsAny<ByIdSpecification<Post>>()))
                .Returns<ByIdSpecification<Post>>(sp => postsWereCreated.SingleOrDefault(sp.IsSatisifiedBy().Compile()));

            return new CommentsService(mockCommentRepository.Object, mockPostRepository.Object);
        }

        #endregion
    }
}