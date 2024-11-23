using FitHub.Platform.Common.Domain;
using FluentValidation;
using FluentValidation.Results;

namespace FitHub.Platform.Common.Service
{
    public interface IValidatorService
    {
        IEnumerable<ValidationError> Validate<TValidatorEntity>(TValidatorEntity entity) 
            where TValidatorEntity : class, new();

        Task ValidateAndThrow<TValidatorEntity>(TValidatorEntity entity)
            where TValidatorEntity : class, new();
    }

    public class ValidatorService : IValidatorService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ValidationError> Validate<TValidatorEntity>(TValidatorEntity entity)
            where TValidatorEntity : class, new()
        {
            IValidator<TValidatorEntity> validator = GetValidatorType(entity);

            ValidationResult result = validator.Validate(entity);

            // Map FluentValidation errors to ValidationError objects
            return result.Errors.Select(error => new ValidationError
            {
                PropertyName = error.PropertyName,
                ErrorMessage = error.ErrorMessage
            });
        }

        public async Task ValidateAndThrow<TValidatorEntity>(TValidatorEntity entity)
            where TValidatorEntity : class, new()
        {
            IValidator<TValidatorEntity> validator = GetValidatorType(entity);

            await validator.ValidateAndThrowAsync(entity);
        }

        private IValidator<TValidatorEntity> GetValidatorType<TValidatorEntity>(TValidatorEntity entity)
            where TValidatorEntity : class, new()
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(typeof(TValidatorEntity));
            IValidator<TValidatorEntity> validator = (IValidator<TValidatorEntity>)_serviceProvider.GetService(genericType)!;

            if (validator is null)
            {
                throw new InvalidOperationException($"No validator found for {typeof(TValidatorEntity).Name}");
            }

            return validator;
        }
    }
}
