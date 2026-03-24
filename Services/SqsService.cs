using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Services
{
    public class SqsService
    {
        private readonly AmazonSQSClient _sqsClient;
        private readonly string _filaPedidoUrl;
        private readonly string _filaPagamentoUrl;
        private readonly string _region;

        public SqsService(IConfiguration config)
        {
            _filaPedidoUrl = config["AWS:SQS:Pedido"];
            _filaPagamentoUrl = config["AWS:SQS:Pagamento"];
            _region = config["AWS:Region"];
            _sqsClient = new AmazonSQSClient(RegionEndpoint.GetBySystemName(_region));
        }
        public async Task ProcessarFilaAsync()
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = _filaPedidoUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20
            };

            var response = await _sqsClient.ReceiveMessageAsync(request);

            var messages = response?.Messages ?? new List<Message>();

            if (messages.Count == 0)
            {
                Console.WriteLine("📭 Fila vazia...");
                return;
            }

            foreach (var message in messages)
            {
                var pedido = JsonSerializer.Deserialize<PedidoCriadoEvent>(message.Body);

                if (pedido == null)
                {
                    Console.WriteLine("❌ Mensagem inválida");
                    continue;
                }

                var aprovado = SimularPagamento(pedido.Valor);

                await EnviarResultadoPagamento(pedido, aprovado);

                await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
                {
                    QueueUrl = _filaPedidoUrl,
                    ReceiptHandle = message.ReceiptHandle
                });
            }
        }
        private async Task EnviarResultadoPagamento(PedidoCriadoEvent pedido, bool aprovado)
        {
            var evento = new
            {
                UsuarioId = pedido.UsuarioId,
                JogoId = pedido.JogoId,
                NomeJogo = pedido.NomeJogo,
                Valor = pedido.Valor,
                Status = aprovado ? "Approved" : "Rejected"
            };

            await _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _filaPagamentoUrl,
                MessageBody = JsonSerializer.Serialize(evento)
            });
        }
        private bool SimularPagamento(decimal valor)
        {
            // regra fake (exemplo)
            return valor > 0;
        }
    }
}
public class PedidoCriadoEvent
{
    public int UsuarioId { get; set; }
    public int JogoId { get; set; }
    public string NomeJogo { get; set; }
    public decimal Valor { get; set; }
}
