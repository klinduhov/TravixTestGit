using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{

    public abstract class ServiceBase<TModel, TException>
        where TModel : IDomainModel
        where TException : Exception
    {
        private readonly IRepository<TModel> repository;
        protected readonly ModelValidatorBase<TModel, TException> Validator;

        protected ServiceBase(IRepository<TModel> repository, ModelValidatorBase<TModel, TException> validator)
        {
            this.repository = repository;
            Validator = validator;
        }

        public async Task<TModel> GetAsync(Guid id)
        {
            return await repository.GetAsync(id);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await repository.GetAllASync();
        }

        protected async Task AddAsync(TModel model, Action<TModel> additionalValidationForAdding)
        {
            Validator.Validate(model);

            additionalValidationForAdding?.Invoke(model);

            await Task.Run(() => repository.AddAsync(model));
        }

        public async Task DeleteAsync(Guid id)
        {
            var model = await GetAsync(id);

            if (model == null)
                throw new Exception("not found for delete");

            await Task.Run(() => repository.DeleteAsync(model));
        }
    }
}