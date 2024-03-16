using Order.Domain.Dtos.Event;
using FluentValidation;

namespace Order.Service.Validators.Event
{
	public class AddOrderValidator : AbstractValidator<OrderDto>
	{
		public AddOrderValidator()
		{
			RuleFor(x => x.EventId)
				.NotEmpty().WithMessage("Informe o id do evento");

			RuleFor(x => x.PaymentMethod)
				.NotEmpty().WithMessage("Informe o metodo de pagamento");

			RuleFor(x => x.Tickets)
				.NotEmpty().WithMessage("Informe a quantidade de tickets que deseja comprar");
		}
	}
}
