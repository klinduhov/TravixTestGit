using System.Collections.Generic;
using System.Linq;
using Moq;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;

namespace TravixTest.Logic.Tests
{
    internal static class TestsHelper
    {
        public static void SetupGetAllModels<T>(this Mock<IRepository<T>> mockRepository, IList<T> modelsTestList)
            where T : IDomainModel
        {
            mockRepository
                .Setup(r => r.GetAll())
                .Returns(() => modelsTestList);
        }

        public static void SetupGetModel<T>(this Mock<IRepository<T>> mockRepository, IList<T> modelsTestList)
            where T : IDomainModel
        {
            mockRepository
                .Setup(r => r.Get(It.IsAny<ByIdSpecification<T>>()))
                .Returns<ByIdSpecification<T>>(sp => modelsTestList.SingleOrDefault(sp.IsSatisifiedBy().Compile()));
        }

        public static void SetupDeleteModel<T>(this Mock<IRepository<T>> mockRepository, IList<T> modelsTestList)
            where T : IDomainModel
        {
            mockRepository
                .Setup(r => r.Delete(It.Is<T>(m => modelsTestList.All(x => x.Id != m.Id))))
                .Returns(false);

            mockRepository
                .Setup(r => r.Delete(It.Is<T>(m => modelsTestList.Any(x => x.Id == m.Id))))
                .Returns(true)
                .Callback<T>(m =>
                {
                    var postToBeDeleted = modelsTestList.Single(x => x.Id == m.Id);
                    modelsTestList.Remove(postToBeDeleted);
                });
        }

        public static void SetupUpdateModel<T>(this Mock<IRepository<T>> mockRepository, IList<T> modelsTestList)
            where T : IDomainModel
        {
            mockRepository
                .Setup(r => r.Update(It.Is<T>(m => modelsTestList.All(x => x.Id != m.Id))))
                .Returns(false);

            mockRepository
                .Setup(r => r.Update(It.Is<T>(m => modelsTestList.Any(x => x.Id == m.Id))))
                .Returns(true)
                .Callback<T>(m =>
                {
                    var postToBeUpdated = modelsTestList.Single(x => x.Id == m.Id);
                    int indexOfPostToBeUpdated = modelsTestList.IndexOf(postToBeUpdated);
                    modelsTestList[indexOfPostToBeUpdated] = m;
                });
        }
    }
}
