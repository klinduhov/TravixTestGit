using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravixTest.Logic.DomainModels;

namespace TravixTest.Logic.Validation
{
    public abstract class ModelValidatorBase<TModel, TException> 
        where TModel : IDomainModel
        where TException  :Exception
    {
        private readonly ICollection<ValidationRule> validationRules = new Collection<ValidationRule>();

        protected void AddRule(Func<TModel, bool> validationPredicate, Func<TException> exceptionConstructor)
        {
            validationRules.Add(new ValidationRule(validationPredicate, exceptionConstructor));
        }

        public void Validate(TModel model)
        {
            foreach (var rule in validationRules)
            {
                if (!rule.ValidationPredicate(model))
                    throw rule.ExceptionConstructor();
            }
        }

        private class ValidationRule            
        {
            public Func<TModel, bool> ValidationPredicate { get; }
            public Func<TException> ExceptionConstructor { get; }

            public ValidationRule(Func<TModel, bool> validationPredicate, Func<TException> exceptionConstructor)
            {
                ValidationPredicate = validationPredicate;
                ExceptionConstructor = exceptionConstructor;
            }
        }
    }
}