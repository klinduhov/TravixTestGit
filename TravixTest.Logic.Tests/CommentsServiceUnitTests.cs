using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public class CommentsServiceUnitTests : ServiceUnitTestsBase<Comment>
    {
        #region Facts

        [Fact]
        public void GetAll_IfAddedComment_ShouldContainAddedOne()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();            

            GetAll_IfAddedModel_ShouldContainAddedOne(commentsServiceTestWrapper.CommentsService, 
                GenerateCommentToBeAdded(commentsServiceTestWrapper));
        }

        [Fact]
        public void GetAll_IfNotAddedComment_ShouldNotContainNotAddedOne()
        {
            GetAll_IfNotAddedModel_ShouldNotContainNotAddedOne(CreateCommentsServiceTestWrapper().CommentsService,
                new Comment(Guid.NewGuid(), Guid.NewGuid(), "not added comment text"));
        }

        [Fact]
        public void Get_IfAddedComment_ShouldReturnNotNullOne()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();

            Get_IfAddedModel_ShouldReturnNotNullOne(commentsServiceTestWrapper.CommentsService,
                GenerateCommentToBeAdded(commentsServiceTestWrapper));
        }

        [Fact]
        public void Get_IfNotAddedComment_ShouldReturnNull()
        {
            Get_IfNotAddedModel_ShouldReturnNull(CreateCommentsServiceTestWrapper().CommentsService, 
                new Comment(Guid.NewGuid(), Guid.NewGuid(), "not added comment text"));
        }

        [Fact]
        public void Get_IfAddedComment_ShouldReturnTheAddedOne()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();

            Get_IfAddedModel_ShouldReturnTheAddedOne(commentsServiceTestWrapper.CommentsService,
                GenerateCommentToBeAdded(commentsServiceTestWrapper));
        }

        [Fact]
        public void GetAllByPost_IfAddedToPostComment_ShouldContainAddedOne()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentToBeAdded = GenerateCommentToBeAdded(commentsServiceTestWrapper);
            commentsServiceTestWrapper.CommentsService.Add(commentToBeAdded);

            Assert.Contains(commentsServiceTestWrapper.CommentsService.GetAllByPost(commentToBeAdded.PostId), 
                c => c.Id == commentToBeAdded.Id);
        }

        [Fact]
        public void GetAllByPost_IfNotAddedToPostAnyComment_ShouldReturnEmpty()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var postWithNoComments = commentsServiceTestWrapper.GetPostWithNoCommentsFromTestSet();

            Assert.Empty(commentsServiceTestWrapper.CommentsService.GetAllByPost(postWithNoComments.Id));
        }

        [Fact]
        public void Add_IfPostIdEmpty_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithEmptyPostId = new Comment(Guid.NewGuid(), Guid.Empty, "comment with empty post id");

            Assert.Throws<CommentValidationException>(() => commentsServiceTestWrapper.CommentsService.Add(commentWithEmptyPostId));
        }

        [Fact]
        public void Add_IfTextEmpty_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithEmptyText = new Comment(Guid.NewGuid(), Guid.NewGuid(), String.Empty);

            Assert.Throws<CommentValidationException>(() => commentsServiceTestWrapper.CommentsService.Add(commentWithEmptyText));
        }

        [Fact]
        public void Add_IfTextContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithWhiteSpacesText = new Comment(Guid.NewGuid(), Guid.NewGuid(), "   ");

            Assert.Throws<CommentValidationException>(() => commentsServiceTestWrapper.CommentsService.Add(commentWithWhiteSpacesText));
        }

        [Fact]
        public void Add_IfCommentWasAlreadyAdded_ShouldThrowException()
        {
            Add_IfModelWasAlreadyAdded_ShouldThrowException(CreateCommentsServiceTestWrapper().CommentsService);
        }

        [Fact]
        public void Add_IfAddedTheSamePost_ShouldThrowException()
        {
            var commentServiceTestWrapper = CreateCommentsServiceTestWrapper();
            Add_IfAddedTheSameModel_ShouldThrowException(commentServiceTestWrapper.CommentsService,
                GenerateCommentToBeAdded(commentServiceTestWrapper));
        }

        [Fact]
        public void Delete_IfCommentWasNotAddedOrAlreadyDeleted_ShouldThrowException()
        {
            Delete_IfModelWasNotAddedOrAlreadyDeleted_ShouldThrowException(
                CreateCommentsServiceTestWrapper().CommentsService,
                new Comment(Guid.NewGuid(), Guid.NewGuid(), "comment not added"));
        }

        [Fact]
        public void Delete_IfAddedCommentDeleted_ShouldNotBeFoundByGetAndGetAll()
        {
            var commentServiceTestWrapper = CreateCommentsServiceTestWrapper();
            Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(commentServiceTestWrapper.CommentsService,
                GenerateCommentToBeAdded(commentServiceTestWrapper));
        }

        [Fact]
        public void Delete_IfAddedCommentDeleted_ShouldNotBeFoundByGetAllByPost()
        {
            var commentServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentToBeDeleted = GenerateCommentToBeAdded(commentServiceTestWrapper);
            var service = commentServiceTestWrapper.CommentsService;
            service.Add(commentToBeDeleted);
            service.Delete(commentToBeDeleted.Id);

            Assert.DoesNotContain(service.GetAllByPost(commentToBeDeleted.PostId), c => c.Id == commentToBeDeleted.Id);
        }

        #endregion

        #region Private methods

        private Comment GenerateCommentToBeAdded(CommentsServiceTestWrapper commentsServiceTestWrapper)
        {
            var firstPostFromTestSet = commentsServiceTestWrapper.GetFirstPostFromTestSet();
            return new Comment(Guid.NewGuid(), firstPostFromTestSet.Id, "added comment text");
        }

        private static CommentsServiceTestWrapper CreateCommentsServiceTestWrapper()
        {
            var commentsWereCreated = new List<Comment>();
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 2).Select(i => 
            {
                var postId = Guid.NewGuid();

                var comments = Enumerable.Range(0, 2)
                    .Select(j => new Comment(Guid.NewGuid(), postId, $"comment {j} for post {i}"))
                    .ToList();

                commentsWereCreated.AddRange(comments);

                return new Post(postId, $"test body {i}", comments);
            }));

            var postWithNoComments = new Post(Guid.NewGuid(), "with no comments test body");
            postsWereCreated.Add(postWithNoComments);

            var mockCommentRepository = new Mock<ICommentsRepository>();

            mockCommentRepository
                .Setup(r => r.GetAll())
                .Returns(() => commentsWereCreated);

            mockCommentRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns<Guid>(id => commentsWereCreated.SingleOrDefault(x => x.Id == id));

            mockCommentRepository
                .Setup(r => r.Delete(It.Is<Comment>(m => commentsWereCreated.All(x => x.Id != m.Id))))
                .Returns(false);

            mockCommentRepository
                .Setup(r => r.Delete(It.Is<Comment>(m => commentsWereCreated.Any(x => x.Id == m.Id))))
                .Returns(true)
                .Callback<Comment>(m =>
                {
                    var commentToBeDeleted = commentsWereCreated.Single(x => x.Id == m.Id);
                    commentsWereCreated.Remove(commentToBeDeleted);
                });

            mockCommentRepository
                .Setup(r => r.Add(It.Is<Comment>(c => 
                    postsWereCreated.All(p => p.Id != c.PostId) || commentsWereCreated.Any(x => x.Id == c.Id))))
                .Returns(false);

            mockCommentRepository
                .Setup(r => r.Add(It.Is<Comment>(c => 
                    postsWereCreated.Any(p => p.Id == c.PostId && commentsWereCreated.All(x => x.Id != c.Id)))))
                .Returns(true)
                .Callback<Comment>(c => commentsWereCreated.Add(c));

            mockCommentRepository
                .Setup(r => r.GetAllByPost(It.IsAny<Guid>()))
                .Returns<Guid>(pid => commentsWereCreated.Where(x => x.PostId == pid));

            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.SetupGetModel(postsWereCreated);
            mockPostRepository.SetupGetAllModels(postsWereCreated);

            return new CommentsServiceTestWrapper(mockCommentRepository, mockPostRepository, postWithNoComments.Id);//new CommentsService(mockCommentRepository.Object, mockPostRepository.Object);
        }

        private class CommentsServiceTestWrapper
        {            
            private readonly IPostRepository postRepository;
            private readonly Guid postWithNoCommentsId;

            public CommentsService CommentsService { get; }

            public CommentsServiceTestWrapper(Mock<ICommentsRepository> mockCommentRepository, 
                Mock<IPostRepository> mockPostRepository, Guid postWithNoCommentsId)
            {
                this.postWithNoCommentsId = postWithNoCommentsId;
                postRepository = mockPostRepository.Object;
                CommentsService = new CommentsService(mockCommentRepository.Object, postRepository);
            }

            public Post GetFirstPostFromTestSet()
            {
                return postRepository.GetAll().First();
            }

            public Post GetPostWithNoCommentsFromTestSet()
            {
                return postRepository.GetAll().First(p => p.Id == postWithNoCommentsId);
            }
        }
        #endregion
    }
}