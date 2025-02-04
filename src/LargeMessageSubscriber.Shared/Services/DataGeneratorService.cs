using Bogus;
using LargeMessageSubscriber.Shared.Models;
using LargeMessageSubscriber.Shared.Services.Contracts;

namespace LargeMessageSubscriber.Shared.Services
{
    public class DataGeneratorService : IDataGeneratorService
    {
        public IEnumerable<MockDataPoint> GenerateMessages()
        {
            var dataPoints = new List<MockDataPoint>();

            var mockDataPoints = new Faker<MockDataPoint>()
               .RuleFor(p => p.Name, f => f.Name.FirstName() + f.Random.Number())
               .RuleFor(p => p.Timestamp, f => f.DateTimeReference)
               .RuleFor(p => p.Value, f => f.Random.Double(1000, 5000))
               .Generate(5000);

            return mockDataPoints;
        }
    }
}
