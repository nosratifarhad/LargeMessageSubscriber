using InfluxDB.Client;
using LargeMessageSubscriber.Persistence.Options;
using LargeMessageSubscriber.Persistence.Repositories.InfluxDb.ConnectionFactory.Contracts;
using Microsoft.Extensions.Options;

namespace LargeMessageSubscriber.Persistence.Repositories.InfluxDb.ConnectionFactory
{
    public class InfluxDbConnectionFactory : IInfluxDbConnectionFactory
    {
        private InfluxDBClient? _client;
        private readonly IOptions<InfluxConnectionOption> _options;

        public InfluxDbConnectionFactory(IOptions<InfluxConnectionOption> options)
        {
            _options = options;
        }

        public InfluxDBClient GetClient()
        {
            if (_client == null)
            {
                _client = InfluxDBClientFactory.Create(_options.Value.Url, _options.Value.Token);
            }
            return _client;
        }

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}
