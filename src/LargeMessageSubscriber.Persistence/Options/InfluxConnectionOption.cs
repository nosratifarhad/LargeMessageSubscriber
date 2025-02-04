namespace LargeMessageSubscriber.Persistence.Options
{
    public class InfluxConnectionOption
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public string Org { get; set; }
        public string Bucket { get; set; }
    }
}
