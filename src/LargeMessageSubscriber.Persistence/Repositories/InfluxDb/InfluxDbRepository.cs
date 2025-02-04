using InfluxDB.Client;
using LargeMessageSubscriber.Shared.Models;
using InfluxDB.Client.Api.Domain;
using LargeMessageSubscriber.Persistence.Options;
using Microsoft.Extensions.Options;
using LargeMessageSubscriber.Persistence.Repositories.InfluxDb.ConnectionFactory.Contracts;
using InfluxDB.Client.Writes;

namespace LargeMessageSubscriber.Persistence.Repositories.InfluxDb
{
    public class InfluxDbRepository
    {
        private readonly InfluxDBClient _client;
        private readonly InfluxConnectionOption _option;

        public InfluxDbRepository(
            IInfluxDbConnectionFactory influxDbConnectionFactory,
            IOptions<InfluxConnectionOption> options)
        {
            _client = influxDbConnectionFactory.GetClient();
            _option = options.Value;
        }

        public async Task InsertMessagesAsync(IEnumerable<MockDataPoint> messages)
        {
            var writeApiAsync = _client.GetWriteApiAsync();

            var points = new List<PointData>();

            foreach (var message in messages)
            {
                points.Add(PointData.Measurement("Message")
                    .Tag("name", message.Name)
                    .Field("value", message.Value)
                    .Field("Timestamp", message.Timestamp)
                    .Field("CreatedAt", DateTime.UtcNow)
                    .Timestamp(message.Timestamp, WritePrecision.Ns));
            }

            await writeApiAsync.WritePointsAsync(points, _option.Bucket, _option.Org);
        }

        public async Task UpdateMessagesAsync(IEnumerable<MockDataPoint> messages)
        {
            var writeApiAsync = _client.GetWriteApiAsync();

            var points = new List<PointData>();

            foreach (var message in messages)
            {
                points.Add(PointData.Measurement("Message")
                    .Tag("name", message.Name)
                    .Field("value", message.Value)
                    .Field("Timestamp", message.Timestamp)
                    .Field("CreatedAt", DateTime.UtcNow)
                    .Timestamp(message.Timestamp, WritePrecision.Ns));
            }

            await writeApiAsync.WritePointsAsync(points, _option.Bucket, _option.Org);
        }
    }
}
