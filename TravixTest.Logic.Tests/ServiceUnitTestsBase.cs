using System;
using System.Linq;
using TravixTest.Logic.DomainModels;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public abstract class ServiceUnitTestsBase<T> where T : IDomainModel
    {
        protected void GetAll_IfAddedModel_ShouldContainAddedOne(IService<T> service, T modelToBeAdded)
        {
            service.Add(modelToBeAdded);

            Assert.Contains(service.GetAll(), m => m.Id == modelToBeAdded.Id);
        }

        public void GetAll_IfNotAddedModel_ShouldNotContainNotAddedOne(IService<T> service, T modelNotAdded)
        {
            Assert.DoesNotContain(service.GetAll(), m => m.Id == modelNotAdded.Id);
        }

        public void Get_IfAddedModel_ShouldReturnNotNullOne(IService<T> service, T modelToBeAdded)
        {
            service.Add(modelToBeAdded);
            var gotModel = service.Get(modelToBeAdded.Id);

            Assert.NotNull(gotModel);
        }

        public void Get_IfNotAddedModel_ShouldReturnNull(IService<T> service, T modelNotAdded)
        {
            //var service = serviceConstructor();
            var gotPost = service.Get(modelNotAdded.Id);

            Assert.Null(gotPost);
        }

        public void Get_IfAddedModel_ShouldReturnTheAddedOne(IService<T> service, T modelToBeAdded)
        {
            service.Add(modelToBeAdded);
            var gotModel = service.Get(modelToBeAdded.Id);

            Assert.Equal(modelToBeAdded.Id, gotModel.Id);
        }

        public void Add_IfModelWasAlreadyAdded_ShouldThrowException(IService<T> service)
        {
            var postAlreadyAdded = service.GetAll().First();

            Assert.Throws<Exception>(() => service.Add(postAlreadyAdded));
        }

        public void Add_IfAddedTheSameModel_ShouldThrowException(IService<T> service, T modelToBeAdded)
        {
            service.Add(modelToBeAdded);

            Assert.Throws<Exception>(() => service.Add(modelToBeAdded));
        }

        public void Delete_IfModelWasNotAddedOrAlreadyDeleted_ShouldThrowException(IService<T> service, T modelNotExisting)
        {
            Assert.Throws<Exception>(() => service.Delete(modelNotExisting.Id));
        }

        public void Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(IService<T> service, T modelToBeDeleted)
        {
            service.Add(modelToBeDeleted);
            service.Delete(modelToBeDeleted.Id);

            Assert.Null(service.Get(modelToBeDeleted.Id));
            Assert.DoesNotContain(service.GetAll(), p => p.Id == modelToBeDeleted.Id);
        }
    }
}