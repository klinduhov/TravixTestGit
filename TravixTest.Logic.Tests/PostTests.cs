using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public class PostTests
    {
        Mock<IPostRepository> mockCreditDecisionService;

        #region Facts

        [Fact]
        public void GetAll_IfAddedPost_ShouldContainAddedOne()
        {
            var service = CreateTestingService();
            var postToBeAdded = new Post(Guid.NewGuid(), "added post body");

            service.Add(postToBeAdded);
            
            Assert.Contains(service.GetAll(), p => p.Id == postToBeAdded.Id);
        }

        [Fact]
        public void GetAll_IfNotAddedPost_ShouldNotContainAddedOne()
        {
            var service = CreateTestingService();
            var postNotAdded = new Post(Guid.NewGuid(), "not added post body");

            Assert.DoesNotContain(service.GetAll(), p => p.Id == postNotAdded.Id);
        }

        [Fact]
        public void Get_IfAddedPost_ShouldReturnNotNullOne()
        {
            var service = CreateTestingService();
            var postToBeAdded = new Post(Guid.NewGuid(), "added post body");

            service.Add(postToBeAdded);

            var gotPost = service.Get(postToBeAdded.Id);

            Assert.NotNull(gotPost);
        }        

        [Fact]
        public void Get_IfNotAddedPost_ShouldReturnNull()
        {
            var service = CreateTestingService();
            var postNotAdded = new Post(Guid.NewGuid(), "not added post body");

            var gotPost = service.Get(postNotAdded.Id);

            Assert.Null(gotPost);
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
            var service = CreateTestingService();
            var postAlreadyAdded = service.GetAll().First();

            Assert.Throws<Exception>(() => service.Add(postAlreadyAdded));
        }

        [Fact]
        public void Add_IfAddedTheSamePost_ShouldThrowException()
        {
            var service = CreateTestingService();
            var postToBeAdded = new Post(Guid.NewGuid(), "added post body");
            service.Add(postToBeAdded);

            Assert.Throws<Exception>(() => service.Add(postToBeAdded));
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

        #endregion

        #region Private methods

        private PostsService CreateTestingService()
        {
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 5).Select(i => new Post(Guid.NewGuid(), $"test body {i}")));

            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(r => r.Add(It.IsAny<Post>())).Callback<Post>((p) => postsWereCreated.Add(p));
            mockPostRepository.Setup(r => r.GetAll()).Returns(() => postsWereCreated);
            mockPostRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns<Guid>((id) => postsWereCreated.SingleOrDefault(p => p.Id == id));

            return new PostsService(mockPostRepository.Object);
        }

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
            var postWithWhiteSpacesBody = new Post(Guid.NewGuid(), "  ");
            var action = GetWriteActionByType(operationType);

            Assert.Throws<PostValidationException>(() => action(service, postWithWhiteSpacesBody));
        }

        private void WriteOperation_IfBodyContainsOnlySpaces_ShouldThrowPostValidationException(WriteOperationTypes operationType)
        {
            var service = CreateTestingService();
            var postWithEmptyBody = new Post(Guid.NewGuid(), string.Empty);
            var action = GetWriteActionByType(operationType);

            Assert.Throws<PostValidationException>(() => action(service, postWithEmptyBody));
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
