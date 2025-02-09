using LargeMessageSubscriber.Domain.LargeMessage.Entities;

namespace LargeMessageSubscriber.Domain.LargeMessage
{
    public interface IRepository : IDisposable
    {
        Task<IEnumerable<object>> GetDataPointPagination(int pageSize, int pageIndex);

        Task<bool> IsExistMessage(string dataPointName);

        Task InsertMessagesAsync(IEnumerable<DataPoint> messages);

        Task UpdateMessagesAsync(IEnumerable<DataPoint> dataPoints);

    }
}
