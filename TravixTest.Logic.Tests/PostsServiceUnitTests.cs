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
        public void Add_IfIdEmpty_ShouldThrowPostValidationException()
        {
            WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public void Add_IfBodyEmpty_ShouldThrowPostValidationException()
        {
            WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public void Add_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes.Add);
        }

        [Fact]
        public void Add_IfPostWasAlreadyAdded_ShouldThrowException()
        {
            Add_IfModelWasAlreadyAdded_ShouldThrowException(CreateTestingService());
        }

        [Fact]
        public void Add_IfAddedTheSamePost_ShouldThrowException()
        {
            Add_IfAddedTheSameModel_ShouldThrowException(CreateTestingService(), new Post(Guid.NewGuid(), "added post body"));
        }

        [Fact]
        public void Update_IfIdEmpty_ShouldThrowPostValidationException()
        {
            WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Update);            
        }

        [Fact]
        public void Update_IfBodyEmpty_ShouldThrowPostValidationException()
        {
            WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes.Update);
        }

        [Fact]
        public void Update_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException()
        {
            WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes.Update);
        }

        [Fact]
        public void Update_IfPostWasNotAddedOrAlreadyDeleted_ShouldThrowException()
        {
            var service = CreateTestingService();
            var postNotExisting = new Post(Guid.NewGuid(), "not existing post body");

            Assert.Throws<Exception>(() => service.Update(postNotExisting));
        }

        [Fact]
        public void Update_IfUpdatedPostBody_GetPostShouldReturnTheSameBody()
        {
            var service = CreateTestingService();
            var postToBeUpdated = service.GetAll().First();
            var updatedPost = new Post(postToBeUpdated.Id, $"updated post body {Guid.NewGuid()}");
            service.Update(updatedPost);
            var gotPost = service.Get(updatedPost.Id);

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

        private static PostsService CreateTestingService()
        {
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 5).Select(i => new Post(Guid.NewGuid(), $"test body {i}")));

            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.SetupGetAllModels(postsWereCreated);
            mockPostRepository.SetupGetModel(postsWereCreated);

            mockPostRepository
                .Setup(r => r.Delete(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(false);

            mockPostRepository
                .Setup(r => r.Delete(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(true)
                .Callback<Post>(p =>
                {
                    var postToBeDeleted = postsWereCreated.Single(x => x.Id == p.Id);
                    postsWereCreated.Remove(postToBeDeleted);
                });

            mockPostRepository
                .Setup(r => r.Update(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(false);

            mockPostRepository
                .Setup(r => r.Update(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(true)
                .Callback<Post>(p =>
                {
                    var postToBeUpdated = postsWereCreated.Single(x => x.Id == p.Id);
                    int indexOfPostToBeUpdated = postsWereCreated.IndexOf(postToBeUpdated);
                    postsWereCreated[indexOfPostToBeUpdated] = p;
                });

            mockPostRepository
                .Setup(r => r.Add(It.Is<Post>(p => postsWereCreated.Any(x => x.Id == p.Id))))
                .Returns(false);

            mockPostRepository
                .Setup(r => r.Add(It.Is<Post>(p => postsWereCreated.All(x => x.Id != p.Id))))
                .Returns(true)
                .Callback<Post>(p => postsWereCreated.Add(p));

            return new PostsService(mockPostRepository.Object);
        }

        #region Private methods        

        private void WriteOperation_IfIdEmpty_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithEmptyBody = new Post(Guid.Empty, "write post body");
            var action = GetWriteActionByType(operationType);

            Assert.Throws<PostValidationException>(() => action(service, postWithEmptyBody));
        }

        private void WriteOperation_IfBodyEmpty_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithEmptyBody = new Post(Guid.NewGuid(), string.Empty);
            var action = GetWriteActionByType(operationType);

            Assert.Throws<PostValidationException>(() => action(service, postWithEmptyBody));            
        }

        private void WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithWhiteSpacesBody = new Post(Guid.NewGuid(), "  ");
            var action = GetWriteActionByType(operationType);

            Assert.Throws<PostValidationException>(() => action(service, postWithWhiteSpacesBody));
        }

        private Action<PostsService, Post> GetWriteActionByType(WriteOperationTypes operationType)
        {
            var changeActionsDictionary = new Dictionary<WriteOperationTypes, Action<PostsService, Post>>
            {
                {WriteOperationTypes.Add, (s, p) => s.Add(p)},
                {WriteOperationTypes.Update,(s, p) => s.Update(p)}
            };

            return changeActionsDictionary[operationType];
        }

        #endregion

        private enum WriteOperationTypes
        {
            Add,
            Update
        }        
    }
}
