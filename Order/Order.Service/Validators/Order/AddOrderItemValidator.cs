using FluentValidation;
using Order.Domain.Dtos.Order;

namespace Order.Service.Validators.Order
{
	public class AddOrderItemValidator : AbstractValidator<AddOrderItemDto>
	{
		public AddOrderItemValidator()
		{
			RuleFor(x => x.Email)
					.NotEmpty().WithMessage("Informe o e-mail em todos os tickets que deseja comprar.");

			RuleFor(x => x.Email)
					.EmailAddress().WithMessage("E-mail informado não é um endereço de e-mail válido.");

			RuleFor(x => x.Name)
					.NotEmpty().WithMessage("Informe o nome em todos os tickets que deseja comprar.");
		}
	}
}
