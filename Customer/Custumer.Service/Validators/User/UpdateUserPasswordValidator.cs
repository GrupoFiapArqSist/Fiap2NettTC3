using Customer.Domain.Dtos.User;
using FluentValidation;

namespace Customer.Service.Validators.User;

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(p => p.NewPassword).ValidPassword();
    }
}
