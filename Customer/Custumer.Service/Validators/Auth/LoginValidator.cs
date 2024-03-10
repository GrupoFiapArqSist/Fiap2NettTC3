using Customer.Domain.Dtos.Auth;
using FluentValidation;

namespace Customer.Service.Validators.Auth;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Informe o usuario");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Informe a senha");
    }
}
