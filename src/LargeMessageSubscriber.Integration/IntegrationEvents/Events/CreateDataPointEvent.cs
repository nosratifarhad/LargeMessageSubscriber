namespace LargeMessageSubscriber.Integration.IntegrationEvents.Events
{
    public class DataPointEvent
    {
        public List<CreateDataPoint> CreateDataPoints { get; set; }
    }

    public class CreateDataPoint
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
