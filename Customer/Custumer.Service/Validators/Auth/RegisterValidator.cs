using Customer.Domain.Dtos.Auth;
using FluentValidation;

namespace Customer.Service.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Informe o usuario");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Informe o primeiro nome");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o email")
            .EmailAddress().WithMessage("Informe um email valido");

        RuleFor(x => x.DocumentType)
            .NotNull().WithMessage("Informe o tipo de documento");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Informe o documento");

        RuleFor(p => p.Password).ValidPassword();
    }
}
