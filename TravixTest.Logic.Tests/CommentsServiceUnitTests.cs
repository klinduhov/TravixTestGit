using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            commentsServiceTestWrapper.CommentsService.AddAsync(commentToBeAdded).Wait();

            Assert.Contains(commentsServiceTestWrapper.CommentsService.GetAllByPostAsync(commentToBeAdded.PostId).SyncResult(), 
                c => c.Id == commentToBeAdded.Id);
        }

        [Fact]
        public void GetAllByPost_IfNotAddedToPostAnyComment_ShouldReturnEmpty()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var postWithNoComments = commentsServiceTestWrapper.GetPostWithNoCommentsFromTestSet();

            Assert.Empty(commentsServiceTestWrapper.CommentsService.GetAllByPostAsync(postWithNoComments.Id).SyncResult());
        }

        [Fact]
        public async Task Add_IfPostIdEmpty_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithEmptyPostId = new Comment(Guid.NewGuid(), Guid.Empty, "comment with empty post id");

            await Assert.ThrowsAsync<CommentValidationException>(async () => 
                await commentsServiceTestWrapper.CommentsService.AddAsync(commentWithEmptyPostId));
        }

        [Fact]
        public async Task Add_IfTextEmpty_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithEmptyText = new Comment(Guid.NewGuid(), Guid.NewGuid(), String.Empty);

            await Assert.ThrowsAsync<CommentValidationException>(async () => 
                await commentsServiceTestWrapper.CommentsService.AddAsync(commentWithEmptyText));
        }

        [Fact]
        public async Task Add_IfTextContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            var commentsServiceTestWrapper = CreateCommentsServiceTestWrapper();
            var commentWithWhiteSpacesText = new Comment(Guid.NewGuid(), Guid.NewGuid(), "   ");

            await Assert.ThrowsAsync<CommentValidationException>(async () => 
                await commentsServiceTestWrapper.CommentsService.AddAsync(commentWithWhiteSpacesText));
        }

        [Fact]
        public async Task  Add_IfCommentWasAlreadyAdded_ShouldThrowException()
        {
            await Add_IfModelWasAlreadyAdded_ShouldThrowException(CreateCommentsServiceTestWrapper().CommentsService);
        }

        [Fact]
        public async Task Add_IfAddedTheSamePost_ShouldThrowException()
        {
            var commentServiceTestWrapper = CreateCommentsServiceTestWrapper();
            await Add_IfAddedTheSameModel_ShouldThrowException(commentServiceTestWrapper.CommentsService,
                GenerateCommentToBeAdded(commentServiceTestWrapper));
        }

        [Fact]
        public async Task Delete_IfCommentWasNotAddedOrAlreadyDeleted_ShouldThrowException()
        {
            await Delete_IfModelWasNotAddedOrAlreadyDeleted_ShouldThrowException(
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
            service.AddAsync(commentToBeDeleted).Wait();
            service.DeleteAsync(commentToBeDeleted.Id).Wait();

            Assert.DoesNotContain(service.GetAllByPostAsync(commentToBeDeleted.PostId).SyncResult(), 
                c => c.Id == commentToBeDeleted.Id);
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
                .Setup(r => r.GetAllASync())
                .ReturnsAsync(() => commentsWereCreated);

            mockCommentRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(commentsWereCreated.SingleOrDefault(x => x.Id == id)));

            mockCommentRepository
                .Setup(r => r.DeleteAsync(It.Is<Comment>(m => commentsWereCreated.All(x => x.Id != m.Id))))
                .Returns(() => Task.FromResult<object>(null));

            mockCommentRepository
                .Setup(r => r.DeleteAsync(It.Is<Comment>(m => commentsWereCreated.Any(x => x.Id == m.Id))))
                .Returns(() => Task.FromResult<object>(null))
                .Callback<Comment>(m =>
                {
                    var commentToBeDeleted = commentsWereCreated.Single(x => x.Id == m.Id);
                    commentsWereCreated.Remove(commentToBeDeleted);
                });

            mockCommentRepository
                .Setup(r => r.AddAsync(It.Is<Comment>(c =>
                    postsWereCreated.All(p => p.Id != c.PostId) || commentsWereCreated.Any(x => x.Id == c.Id))))
                .Returns(() => Task.FromResult<object>(null));

            mockCommentRepository
                .Setup(r => r.AddAsync(It.Is<Comment>(c => 
                    postsWereCreated.Any(p => p.Id == c.PostId && commentsWereCreated.All(x => x.Id != c.Id)))))
                .Returns(() => Task.FromResult<object>(null))
                .Callback<Comment>(c => commentsWereCreated.Add(c));

            mockCommentRepository
                .Setup(r => r.GetAllByPostAsync(It.IsAny<Guid>()))
                .Returns((Guid pid) => Task.FromResult(commentsWereCreated.Where(x => x.PostId == pid)));

            var mockPostRepository = new Mock<IPostsRepository>();
            mockPostRepository.SetupGetModel(postsWereCreated);
            mockPostRepository.SetupGetAllModels(postsWereCreated);

            return new CommentsServiceTestWrapper(mockCommentRepository, mockPostRepository, postWithNoComments.Id);//new CommentsService(mockCommentRepository.Object, mockPostRepository.Object);
        }

        private class CommentsServiceTestWrapper
        {            
            private readonly IPostsRepository postRepository;
            private readonly Guid postWithNoCommentsId;

            public CommentsService CommentsService { get; }

            public CommentsServiceTestWrapper(Mock<ICommentsRepository> mockCommentRepository, 
                Mock<IPostsRepository> mockPostRepository, Guid postWithNoCommentsId)
            {
                this.postWithNoCommentsId = postWithNoCommentsId;
                postRepository = mockPostRepository.Object;
                CommentsService = new CommentsService(mockCommentRepository.Object, postRepository);
            }

            public Post GetFirstPostFromTestSet()
            {
                return postRepository.GetAllASync().SyncResult().First();
            }

            public Post GetPostWithNoCommentsFromTestSet()
            {
                return postRepository.GetAllASync().SyncResult().First(p => p.Id == postWithNoCommentsId);
            }
        }
        #endregion
    }
}