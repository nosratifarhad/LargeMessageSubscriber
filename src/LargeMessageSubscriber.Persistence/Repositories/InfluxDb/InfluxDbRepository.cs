using InfluxDB.Client;
using LargeMessageSubscriber.Shared.Models;
using InfluxDB.Client.Api.Domain;
using LargeMessageSubscriber.Persistence.Options;
using Microsoft.Extensions.Options;
using LargeMessageSubscriber.Persistence.Repositories.InfluxDb.ConnectionFactory.Contracts;
using InfluxDB.Client.Writes;
using LargeMessageSubscriber.Domain.LargeMessage;
using LargeMessageSubscriber.Domain.LargeMessage.Entities;

namespace LargeMessageSubscriber.Persistence.Repositories.InfluxDb
{
    public class InfluxDbRepository : IInfluxDbRepository
    {
        private readonly InfluxDBClient _client;
        private readonly InfluxConnectionOption _option;
        private bool disposedValue;

        public InfluxDbRepository(
            IInfluxDbConnectionFactory influxDbConnectionFactory,
            IOptions<InfluxConnectionOption> options)
        {
            _client = influxDbConnectionFactory.GetClient();
            _option = options.Value;
        }

        public async Task InsertMessagesAsync(IEnumerable<DataPoint> dataPoints)
        {
            var writeApiAsync = _client.GetWriteApiAsync();

            var points = new List<PointData>();

            foreach (var dataPoint in dataPoints)
            {
                points.Add(PointData.Measurement("Message")
                    .Tag("name", dataPoint.Name)
                    .Field("value", dataPoint.Value)
                    .Field("Timestamp", dataPoint.Timestamp)
                    .Field("CreatedAt", DateTime.UtcNow)
                    .Timestamp(dataPoint.Timestamp, WritePrecision.Ns));
            }

            await writeApiAsync.WritePointsAsync(points, _option.Bucket, _option.Org);
        }

        public async Task UpdateMessagesAsync(IEnumerable<DataPoint> dataPoints)
        {
            var writeApiAsync = _client.GetWriteApiAsync();

            var points = new List<PointData>();

            foreach (var dataPoint in dataPoints)
            {
                points.Add(PointData.Measurement("Message")
                    .Tag("name", dataPoint.Name)
                    .Field("value", dataPoint.Value)
                    .Field("Timestamp", dataPoint.Timestamp)
                    .Field("CreatedAt", DateTime.UtcNow)
                    .Timestamp(dataPoint.Timestamp, WritePrecision.Ns));
            }

            await writeApiAsync.WritePointsAsync(points, _option.Bucket, _option.Org);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~InfluxDbRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
