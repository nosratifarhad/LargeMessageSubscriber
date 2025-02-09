using LargeMessageSubscriber.Domain.LargeMessage.Entities;

namespace LargeMessageSubscriber.Domain.LargeMessage
{
    public interface IInfluxDbRepository : IDisposable
    {
        Task InsertMessagesAsync(IEnumerable<DataPoint> messages);

        Task UpdateMessagesAsync(IEnumerable<DataPoint> dataPoints);
    }
}
