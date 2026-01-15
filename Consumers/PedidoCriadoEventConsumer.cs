using CloudGames.Contracts.Events;
using MassTransit;

namespace Consumers
{
    public class PedidoCriadoEventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PedidoCriadoEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var pedido = context.Message;

            var aprovado = SimularPagamento(pedido.Price);

            await _publishEndpoint.Publish(new PaymentProcessedEvent(
                pedido.UserId,
                pedido.GameId,
                pedido.GameName,
                pedido.Price,
                aprovado ? PaymentStatus.Approved : PaymentStatus.Rejected
            ));
        }

        private bool SimularPagamento(decimal valor)
        {
            // regra fake (exemplo)
            return valor > 0;
        }
    }
}
