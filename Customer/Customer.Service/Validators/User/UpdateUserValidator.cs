using Customer.Domain.Dtos.User;
using FluentValidation;

namespace Customer.Service.Validators.User;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Informe o usuario");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Informe o primeiro nome");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o email")
            .EmailAddress().WithMessage("Informe um email valido");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Informe o documento");

        RuleFor(x => x.PhoneNumber)
            .Length(11).WithMessage("Telefone deve ter 11 caracteres")
            .Matches(@"^[0-9]*$").WithMessage("Telefone deve conter apenas números");
    }
}
