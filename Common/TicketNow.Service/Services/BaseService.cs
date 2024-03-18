using FluentValidation;
using FluentValidation.Results;
using TicketNow.Domain.Interfaces.Services;

namespace TicketNow.Service.Services;

public class BaseService : IBaseService
{
    public ValidationResult Validate<T>(T obj, AbstractValidator<T> validator)
    {
        if (obj == null)
            throw new Exception("Registros não detectados!");

        return validator.Validate(obj);
    }
}
