using System;
using System.Collections.Generic;
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

        public TModel Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<TModel> GetAll()
        {
            return repository.GetAll();
        }

        protected bool Add(TModel model, Action<TModel> additionalValidationForAdding)
        {
            Validator.Validate(model);

            additionalValidationForAdding?.Invoke(model);

            return repository.Add(model);
        }


        //protected bool Update(TModel model, Action<TModel> additionalActionForAdding)
        //{
        //    Validator.Validate(model);

        //    var oldModel = Get(model.Id);

        //    if (oldModel == null)
        //        throw new Exception("not found for update");

        //    additionalActionForAdding?.Invoke(model);

        //    return repository.Update(model);
        //}

        public bool Delete(Guid id)
        {
            var model = Get(id);

            if (model == null)
                throw new Exception("not found for delete");

            return repository.Delete(model);
        }
    }
}