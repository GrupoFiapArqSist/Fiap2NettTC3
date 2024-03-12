using FluentValidation;

namespace Event.Service.Validators
{
    // https://www.locktar.nl/programming/net-core/creating-custom-validators-with-fluentvalidation/
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> ValidPassword<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Informe a senha")
                .MinimumLength(8).WithMessage("Sua senha deve conter no minimo 8 caracteres")
                .Matches(@"[A-Z]+").WithMessage("Sua senha deve conter no minimo uma letra maiúscula")
                .Matches(@"[a-z]+").WithMessage("Sua senha deve conter no minimo uma letra minúscula")
                .Matches(@"[0-9]+").WithMessage("Sua senha deve conter no minimo uma número")
                .Matches("(\\W)+").WithMessage("Sua senha deve conter no minimo um caractere especial");
        }
    }
}
