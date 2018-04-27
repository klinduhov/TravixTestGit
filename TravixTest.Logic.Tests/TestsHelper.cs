using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Tests
{
    internal static class TestsHelper
    {
        public static void SetupGetAllModels(this Mock<IPostsRepository> mockRepository, IList<Post> modelsTestList)
        {
            mockRepository
                .Setup(r => r.GetAllASync())
                .ReturnsAsync(() => modelsTestList);
        }

        public static void SetupGetModel(this Mock<IPostsRepository> mockRepository, IList<Post> modelsTestList)
        {
            mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()).SyncResult())
                .Returns<Guid>(id => modelsTestList.SingleOrDefault(x => x.Id == id));
        }

        public static T SyncResult<T>(this Task<T> task)
        {
            task.Wait();

            return task.Result;
        }
    }
}
