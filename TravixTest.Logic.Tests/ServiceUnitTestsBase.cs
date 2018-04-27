using System;
using System.Linq;
using System.Threading.Tasks;
using TravixTest.Logic.DomainModels;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public abstract class ServiceUnitTestsBase<T> where T : IDomainModel
    {
        protected void GetAll_IfAddedModel_ShouldContainAddedOne(IService<T> service, T modelToBeAdded)
        {
            service.AddAsync(modelToBeAdded).Wait();

            Assert.Contains(service.GetAllAsync().SyncResult(), m => m.Id == modelToBeAdded.Id);
        }

        public void GetAll_IfNotAddedModel_ShouldNotContainNotAddedOne(IService<T> service, T modelNotAdded)
        {
            Assert.DoesNotContain(service.GetAllAsync().SyncResult(), m => m.Id == modelNotAdded.Id);
        }

        public void Get_IfAddedModel_ShouldReturnNotNullOne(IService<T> service, T modelToBeAdded)
        {
            service.AddAsync(modelToBeAdded).Wait();
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
            service.AddAsync(modelToBeAdded).Wait();
            var gotModel = service.GetAsync(modelToBeAdded.Id).SyncResult();

            Assert.Equal(modelToBeAdded.Id, gotModel.Id);
        }

        public async Task Add_IfModelWasAlreadyAdded_ShouldThrowException(IService<T> service)
        {
            //var posts = await service.GetAllAsync().SyncResult();
            var postAlreadyAdded = service.GetAllAsync().SyncResult().First();

            await Assert.ThrowsAsync<Exception>(async () => await service.AddAsync(postAlreadyAdded));
        }

        public async Task Add_IfAddedTheSameModel_ShouldThrowException(IService<T> service, T modelToBeAdded)
        {
            await service.AddAsync(modelToBeAdded);

            await Assert.ThrowsAsync<Exception>(async () => await service.AddAsync(modelToBeAdded));
        }

        public async Task Delete_IfModelWasNotAddedOrAlreadyDeleted_ShouldThrowException(IService<T> service, T modelNotExisting)
        {
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(modelNotExisting.Id));
        }

        public void Delete_IfAddedModelDeleted_ShouldNotBeFoundByGetAndGetAll(IService<T> service, T modelToBeDeleted)
        {
            service.AddAsync(modelToBeDeleted).Wait();
            service.DeleteAsync(modelToBeDeleted.Id).Wait();

            Assert.Null(service.GetAsync(modelToBeDeleted.Id).SyncResult());
            Assert.DoesNotContain(service.GetAllAsync().SyncResult(), p => p.Id == modelToBeDeleted.Id);
        }
    }
}