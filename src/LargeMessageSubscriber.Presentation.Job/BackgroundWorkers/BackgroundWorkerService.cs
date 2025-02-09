using LargeMessageSubscriber.Integration.IntegrationEvents.Events;
using LargeMessageSubscriber.Shared.Models;
using LargeMessageSubscriber.Shared.Services.Contracts;
using MassTransit;

namespace LargeMessageSubscriber.Presentation.Job.BackgroundWorkers
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly IDataGeneratorService _dataGeneratorService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BackgroundWorkerService> _logger;

        public BackgroundWorkerService(
            IDataGeneratorService dataGeneratorService,
            IPublishEndpoint publishEndpoint,
            ILogger<BackgroundWorkerService> logger)
        {
            _dataGeneratorService = dataGeneratorService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var mockDataPoint = _dataGeneratorService.GenerateMessages();

                await PublishAsync(mockDataPoint);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task PublishAsync(IEnumerable<MockDataPoint> mockDataPoints)
        {
            var dataPointEvent = new DataPointEvent();

            foreach (var mockDataPoint in mockDataPoints)
            {
                dataPointEvent.CreateDataPoint.Add(new CreateDataPoint()
                {
                    Name = mockDataPoint.Name,
                    Timestamp = mockDataPoint.Timestamp,
                    Value = mockDataPoint.Value,
                });
            }

            await _publishEndpoint.Publish(dataPointEvent);

            _logger.LogInformation("Publish");
        }
    }

}
