namespace LargeMessageSubscriber.Domain.LargeMessage.Entities
{
    public class DataPoint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
