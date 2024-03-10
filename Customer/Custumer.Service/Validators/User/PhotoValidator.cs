using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Customer.Service.Validators.User;

public class PhotoValidator : AbstractValidator<IFormFile>
{
    public PhotoValidator()
    {
        const int MaxFileSize = 1 * 1024 * 1024; // 1 MB

        RuleFor(x => x).NotNull()
            .WithMessage("O arquivo é obrigatório");

        RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(MaxFileSize)
            .WithMessage("Tamanho do arquivo é maior que o permitido");

        RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
            .WithMessage("O arquivo deve ser uma imagem");
    }
}
