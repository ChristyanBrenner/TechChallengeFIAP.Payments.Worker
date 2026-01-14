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

            //Console.WriteLine("💳 Processando pagamento...");
            //Console.WriteLine($"Pedido: {pedido.Id} | Usuário: {pedido.UserId} | Jogo: {pedido.GameId}");

            // 🔥 SIMULA pagamento
            var aprovado = SimularPagamento(pedido.Price);

            await _publishEndpoint.Publish(new PaymentProcessedEvent(
                pedido.UserId,
                pedido.GameId,
                pedido.GameName,
                aprovado ? PaymentStatus.Approved : PaymentStatus.Rejected
            ));

            //Console.WriteLine(aprovado
            //    ? "✅ Pagamento aprovado"
            //    : "❌ Pagamento rejeitado");
        }

        private bool SimularPagamento(decimal valor)
        {
            // regra fake (exemplo)
            return valor > 0;
        }
    }
}
