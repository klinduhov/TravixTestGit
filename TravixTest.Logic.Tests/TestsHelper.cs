using System;
using System.Collections.Generic;
using System.Linq;
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
                .Setup(r => r.GetAll())
                .Returns(() => modelsTestList);
        }

        public static void SetupGetModel(this Mock<IPostsRepository> mockRepository, IList<Post> modelsTestList)
        {
            mockRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns<Guid>(id => modelsTestList.SingleOrDefault(x => x.Id == id));
        }
    }
}
