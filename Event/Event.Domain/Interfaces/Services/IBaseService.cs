using FluentValidation;
using FluentValidation.Results;

namespace Event.Domain.Interfaces.Services
{
    public interface IBaseService
    {
        ValidationResult Validate<T>(T obj, AbstractValidator<T> validator);
    }
}
