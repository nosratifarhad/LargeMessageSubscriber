using LargeMessageSubscriber.Domain.LargeMessage;
using LargeMessageSubscriber.Domain.LargeMessage.Entities;
using LargeMessageSubscriber.Integration.IntegrationEvents.Events;
using MassTransit;

namespace LargeMessageSubscriber.Integration.IntegrationEvents
{
    public class DataPointEventConsumer : IConsumer<DataPointEvent>
    {
        private readonly IRepository _repository;
        private readonly IInfluxDbRepository _influxDbRepository;

        public DataPointEventConsumer(IRepository repository, IInfluxDbRepository influxDbRepository)
        {
            _influxDbRepository = influxDbRepository;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<DataPointEvent> context)
        {
            await SaveToInfluxDb(context.Message.CreateDataPoint);

            await SaveToSqlServer(context.Message.CreateDataPoint);

            Console.WriteLine("Processed and saved 5000 data points.");
        }

        private async Task SaveToInfluxDb(IEnumerable<CreateDataPoint> createDataPoints)
        {
            var dataPoints = new List<DataPoint>();

            foreach (var dataPoint in createDataPoints)
            {
                dataPoints.Add(new DataPoint()
                {
                    Id = Guid.NewGuid(),
                    Name = dataPoint.Name,
                    Timestamp = dataPoint.Timestamp,
                    Value = dataPoint.Value,
                    CreateAt = DateTime.UtcNow
                });
            }

            await _influxDbRepository.InsertMessagesAsync(dataPoints).ConfigureAwait(false);
        }

        private async Task SaveToSqlServer(IEnumerable<CreateDataPoint> createDataPoints)
        {
            var dataPoints = new List<DataPoint>();

            foreach (var dataPoint in createDataPoints)
            {
                var isExistMessage = await _repository.IsExistMessage(dataPoint.Name);
                if (isExistMessage)
                {
                    dataPoints.Add(new DataPoint()
                    {
                        Id = Guid.NewGuid(),
                        Name = dataPoint.Name,
                        Timestamp = dataPoint.Timestamp,
                        Value = dataPoint.Value,
                        CreateAt = DateTime.UtcNow
                    });

                    await _repository.InsertMessagesAsync(dataPoints);
                }
                else
                {
                    dataPoints.Add(new DataPoint()
                    {
                        Name = dataPoint.Name,
                        Timestamp = dataPoint.Timestamp,
                        Value = dataPoint.Value,
                        UpdateAt = DateTime.UtcNow
                    });

                    await _repository.UpdateMessagesAsync(dataPoints);
                }
            }
        }
    }
}
