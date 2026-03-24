using Services;

namespace CloudGames.Payments.Worker
{
    public class Worker : BackgroundService
    {
        private readonly SqsService _sqsService;

        public Worker(SqsService sqsService)
        {
            _sqsService = sqsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _sqsService.ProcessarFilaAsync();

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
