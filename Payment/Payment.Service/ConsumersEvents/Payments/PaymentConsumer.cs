using MassTransit;

public class PaymentConsumer : IConsumer<PaymentsDto>
{
    public PaymentConsumer()
    {

    }

    public Task Consume(ConsumeContext<PaymentsDto> context)
    {
        //Processar

        //Gravar
        //Enviar
    }
}