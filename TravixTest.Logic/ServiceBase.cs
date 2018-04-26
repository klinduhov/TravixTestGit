using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TravixTest.Logic.Contracts;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.DomainSpecifications;
using TravixTest.Logic.Specifications;
using TravixTest.Logic.Validation;

namespace TravixTest.Logic
{

    public abstract class ServiceBase<TModel, TException>
        where TModel : IDomainModel
        where TException : Exception
    {
        protected readonly IRepository<TModel> Repository;
        protected readonly ModelValidatorBase<TModel, TException> Validator;

        protected ServiceBase(IRepository<TModel> repository, ModelValidatorBase<TModel, TException> validator)
        {
            Repository = repository;
            Validator = validator;
        }

        public TModel Get(Guid id)
        {
            return Repository.Get(new Collection<DomainSpecificationBase> { new ByIdDomainSpecification(id) });
        }

        public IEnumerable<TModel> GetAll()
        {
            return Repository.GetAll();
        }

        protected bool Add(TModel model, Action<TModel> additionalValidationForAdding)
        {
            Validator.Validate(model);

            additionalValidationForAdding?.Invoke(model);

            return Repository.Add(model);
        }


        protected bool Update(TModel model, Action<TModel> additionalActionForAdding)
        {
            Validator.Validate(model);

            var oldModel = Get(model.Id);

            if (oldModel == null)
                throw new Exception("not found for update");

            additionalActionForAdding?.Invoke(model);

            return Repository.Update(model);
        }

        public bool Delete(Guid id)
        {
            var model = Get(id);

            if (model == null)
                throw new Exception("not found for delete");

            return Repository.Delete(model);
        }
    }
}