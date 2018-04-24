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
        public void Get_IfAddedPost_ShouldReturnTheAddedOne()
        {
            var service = CreateTestingService();
            var postToBeAdded = new Post(Guid.NewGuid(), "added post body");
            service.Add(postToBeAdded);
            var gotPost = service.Get(postToBeAdded.Id);

            Assert.Equal(postToBeAdded.Id, gotPost.Id);
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
            var service = CreateTestingService();
            var postNotExisting = new Post(Guid.NewGuid(), "not existing post body");

            Assert.Throws<Exception>(() => service.Delete(postNotExisting.Id));
        }

        [Fact]
        public void Delete_IfAddedPost_ShouldNotBeFoundByGetAndGetAll()
        {
            var service = CreateTestingService();
            var postToBeDeleted = new Post(Guid.NewGuid(), "to be deleted post body");
            service.Add(postToBeDeleted);
            service.Delete(postToBeDeleted.Id);

            Assert.Null(service.Get(postToBeDeleted.Id));
            Assert.DoesNotContain(service.GetAll(), p => p.Id == postToBeDeleted.Id);
        }

        #endregion

        #region Private methods

        private PostsService CreateTestingService()
        {
            var postsWereCreated = new List<Post>();
            postsWereCreated.AddRange(Enumerable.Range(0, 5).Select(i => new Post(Guid.NewGuid(), $"test body {i}")));

            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository
                .Setup(r => r.Add(It.IsAny<Post>()))
                .Returns(true)
                .Callback<Post>((p) => postsWereCreated.Add(p));

            mockPostRepository
                .Setup(r => r.GetAll())
                .Returns(() => postsWereCreated);

            mockPostRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns<Guid>((id) => postsWereCreated.SingleOrDefault(p => p.Id == id));

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
                    postsWereCreated[indexOfPostToBeUpdated] = new Post(p.Id, p.Body);
                });

            mockPostRepository
                .Setup(r => r.Delete(It.Is<Guid>(id => postsWereCreated.All(x => x.Id != id))))
                .Returns(false);

            mockPostRepository
                .Setup(r => r.Delete(It.Is<Guid>(id => postsWereCreated.Any(x => x.Id == id))))
                .Returns(true)
                .Callback<Guid>(id =>
                {
                    var postToBeDeleted = postsWereCreated.Single(x => x.Id == id);
                    postsWereCreated.Remove(postToBeDeleted);
                });
            
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

        private class PostComparerIgnoringComments : IEqualityComparer<Post>
        {
            public bool Equals(Post x, Post y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.Body.Equals(y.Body, StringComparison.Ordinal);
            }

            public int GetHashCode(Post obj)
            {
                return new { obj.Id, obj.Body }.GetHashCode();
            }
        }

        private enum WriteOperationTypes
        {
            Add,
            Update
        }               
    }
}
