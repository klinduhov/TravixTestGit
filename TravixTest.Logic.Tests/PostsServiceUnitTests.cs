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
    public class PostsServiceUnitTests : ServiceUnitTestsBase<Post>
    {
        #region Facts

        [Fact]
        public void GetAll_IfAddedPost_ShouldContainAddedOne()
        {
            GetAll_IfAddedModel_ShouldContainAddedOne(CreateTestingService(), new Post(Guid.NewGuid(), "added post body"));
        }

        [Fact]
        public void GetAll_IfNotAddedPost_ShouldNotContainNotAddedOne()
        {
            GetAll_IfNotAddedModel_ShouldNotContainNotAddedOne(CreateTestingService(), new Post(Guid.NewGuid(), "not added post body"));
        }

        [Fact]
        public void Get_IfAddedPost_ShouldReturnNotNullOne()
        {
            Get_IfAddedModel_ShouldReturnNotNullOne(CreateTestingService(), new Post(Guid.NewGuid(), "added post body"));
        }        

        [Fact]
        public void Get_IfNotAddedPost_ShouldReturnNull()
        {
            Get_IfNotAddedModel_ShouldReturnNull(CreateTestingService(), new Post(Guid.NewGuid(), "not added post body"));
        }

        [Fact]
        public void Get_IfAddedPost_ShouldReturnTheAddedOne()
        {
            Get_IfAddedModel_ShouldReturnTheAddedOne(CreateTestingService(), new Post(Guid.NewGuid(), "added post body"));
        }

        [Fact]
        public async Task Add_IfIdEmpty_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public async Task Add_IfBodyEmpty_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public async Task Add_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public async Task Add_IfPostWasAlreadyAdded_ShouldThrowException()
        {
            await Add_IfModelWasAlreadyAdded_ShouldThrowException(CreateTestingService());
        }

        [Fact]
        public async Task Add_IfAddedTheSamePost_ShouldThrowException()
        {
            await Add_IfAddedTheSameModel_ShouldThrowException(CreateTestingService(), new Post(Guid.NewGuid(), "added post body"));
        }

        [Fact]
        public async Task Update_IfIdEmpty_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Update);            
        }

        [Fact]
        public async Task Update_IfBodyEmpty_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Update);
        }

        [Fact]
        public async Task Update_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            await WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes.Update);
        }

        [Fact]
        public async Task Update_IfPostWasNotAddedOrAlreadyDeleted_ShouldThrowException()
        {
            var service = CreateTestingService();
            var postNotExisting = new Post(Guid.NewGuid(), "not existing post body");

            await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(postNotExisting));
        }

        [Fact]
        public void Update_IfUpdatedPostBody_GetPostShouldReturnTheSameBody()
        {
            var service = CreateTestingService();
            var postToBeUpdated = service.GetAllAsync().SyncResult().First();
            var updatedPost = new Post(postToBeUpdated.Id, $"updated post body {Guid.NewGuid()}");
            service.UpdateAsync(updatedPost).Wait();
            var gotPost = service.GetAsync(updatedPost.Id).SyncResult();

            Assert.Equal(updatedPost.Body, gotPost.Body);
        }

        [Fact]
        public void Delete_IfPostWasNotAddedOrAlreadyDeleted_ShouldThrowException()
        {
            Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(CreateTestingService(), 
                new Post(Guid.NewGuid(), "not existing post body"));
        }

        [Fact]
        public void Delete_IfAddedPostDeleted_ShouldNotBeFoundByGetAndGetAll()
        {
            Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(CreateTestingService(),
                new Post(Guid.NewGuid(), "to be deleted post body"));
        }

        #endregion

        #region Private methods        

        private static PostsService CreateTestingService()
        {
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 5).Select(i => new Post(Guid.NewGuid(), $"test body {i}")));

            var mockPostRepository = new Mock<IPostsRepository>();

            mockPostRepository.SetupGetAllModels(postsWereCreated);
            mockPostRepository.SetupGetModel(postsWereCreated);

            mockPostRepository
                .Setup(r => r.DeleteAsync(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(() => Task.FromResult<object>(null));
            //.Returns(false);

            mockPostRepository
                .Setup(r => r.DeleteAsync(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(() => Task.FromResult<object>(null))
                .Callback<Post>(p =>
                {
                    var postToBeDeleted = postsWereCreated.Single(x => x.Id == p.Id);
                    postsWereCreated.Remove(postToBeDeleted);
                });

            mockPostRepository
                .Setup(r => r.UpdateAsync(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(() => Task.FromResult<object>(null));
            //.Returns(false);

            mockPostRepository
                .Setup(r => r.UpdateAsync(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(() => Task.FromResult<object>(null))
                .Callback<Post>(p =>
                {
                    var postToBeUpdated = postsWereCreated.Single(x => x.Id == p.Id);
                    int indexOfPostToBeUpdated = postsWereCreated.IndexOf(postToBeUpdated);
                    postsWereCreated[indexOfPostToBeUpdated] = p;
                });

            mockPostRepository
                .Setup(r => r.AddAsync(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(() => Task.FromResult<object>(null));
            //.Returns(false);

            mockPostRepository
                .Setup(r => r.AddAsync(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(() => Task.FromResult<object>(null))
                .Callback<Post>(p =>
                {
                    postsWereCreated.Add(p);
                });

            return new PostsService(mockPostRepository.Object);
        }

        private async Task WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithEmptyBody = new Post(Guid.Empty, "write post body");

            switch (operationType)
            {
                case WriteOperationTypes.Add:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.AddAsync(postWithEmptyBody));
                    break;
                case WriteOperationTypes.Update:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.UpdateAsync(postWithEmptyBody));
                    break;
            }
        }

        private async Task WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithEmptyBody = new Post(Guid.NewGuid(), string.Empty);

            switch (operationType)
            {
                case WriteOperationTypes.Add:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.AddAsync(postWithEmptyBody));
                    break;
                case WriteOperationTypes.Update:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.UpdateAsync(postWithEmptyBody));
                    break;
            }
            
        }

        private async Task WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithWhiteSpacesBody = new Post(Guid.NewGuid(), "  ");

            switch (operationType)
            {
                case WriteOperationTypes.Add:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.AddAsync(postWithWhiteSpacesBody));
                    break;
                case WriteOperationTypes.Update:
                    await Assert.ThrowsAsync<PostValidationException>(async () => await service.UpdateAsync(postWithWhiteSpacesBody));
                    break;
            }
        }

        #endregion

        private enum WriteOperationTypes
        {
            Add,
            Update
        }
    }
}
