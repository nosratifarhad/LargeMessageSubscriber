using LargeMessageSubscriber.Integration.IntegrationEvents.Events;
using MassTransit;

namespace LargeMessageSubscriber.Integration.IntegrationEvents
{
    internal class DataPointEventConsumer : IConsumer<DataPointEvent>
    {
        public Task Consume(ConsumeContext<DataPointEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
