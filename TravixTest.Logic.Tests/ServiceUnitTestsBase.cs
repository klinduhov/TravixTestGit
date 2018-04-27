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
            service.AddAsync(modelToBeAdded);

            Assert.Contains(service.GetAllAsync().SyncResult(), m => m.Id == modelToBeAdded.Id);
        }

        public void GetAll_IfNotAddedModel_ShouldNotContainNotAddedOne(IService<T> service, T modelNotAdded)
        {
            Assert.DoesNotContain(service.GetAllAsync().SyncResult(), m => m.Id == modelNotAdded.Id);
        }

        public void Get_IfAddedModel_ShouldReturnNotNullOne(IService<T> service, T modelToBeAdded)
        {
            service.AddAsync(modelToBeAdded);
            var gotModel = service.GetAsync(modelToBeAdded.Id).SyncResult();

            Assert.NotNull(gotModel);
        }

        public void Get_IfNotAddedModel_ShouldReturnNull(IService<T> service, T modelNotAdded)
        {
            //var service = serviceConstructor();
            var gotPost = service.GetAsync(modelNotAdded.Id).SyncResult();

            Assert.Null(gotPost);
        }

        public void Get_IfAddedModel_ShouldReturnTheAddedOne(IService<T> service, T modelToBeAdded)
        {
            service.AddAsync(modelToBeAdded);
            var gotModel = service.GetAsync(modelToBeAdded.Id).SyncResult();

            Assert.Equal(modelToBeAdded.Id, gotModel.Id);
        }

        public void Add_IfModelWasAlreadyAdded_ShouldThrowException(IService<T> service)
        {
            var postAlreadyAdded = service.GetAllAsync().SyncResult().First();

            Assert.Throws<Exception>(() => service.AddAsync(postAlreadyAdded).Wait());
        }

        public void Add_IfAddedTheSameModel_ShouldThrowException(IService<T> service, T modelToBeAdded)
        {
            service.AddAsync(modelToBeAdded);

            Assert.Throws<Exception>(() => service.AddAsync(modelToBeAdded).Wait());
        }

        public void Delete_IfModelWasNotAddedOrAlreadyDeleted_ShouldThrowException(IService<T> service, T modelNotExisting)
        {
            Assert.Throws<Exception>(() => service.DeleteAsync(modelNotExisting.Id).Wait());
        }

        public void Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(IService<T> service, T modelToBeDeleted)
        {
            service.AddAsync(modelToBeDeleted);
            service.DeleteAsync(modelToBeDeleted.Id);

            Assert.Null(service.GetAsync(modelToBeDeleted.Id));
            Assert.DoesNotContain(service.GetAllAsync().SyncResult(), p => p.Id == modelToBeDeleted.Id);
        }
    }
}