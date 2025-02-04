using LargeMessageSubscriber.Shared.Models;

namespace LargeMessageSubscriber.Shared.Services.Contracts
{
    public interface IDataGeneratorService
    {
        IEnumerable<MockDataPoint> GenerateMessages();
    }
}
