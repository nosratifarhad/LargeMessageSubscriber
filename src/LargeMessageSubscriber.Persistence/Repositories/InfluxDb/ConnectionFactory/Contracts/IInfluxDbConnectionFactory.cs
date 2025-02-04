using InfluxDB.Client;

namespace LargeMessageSubscriber.Persistence.Repositories.InfluxDb.ConnectionFactory.Contracts
{
    public interface IInfluxDbConnectionFactory : IDisposable
    {
        InfluxDBClient GetClient();
    }
}
