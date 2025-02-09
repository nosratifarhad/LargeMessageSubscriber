using LargeMessageSubscriber.Application.Common;

namespace LargeMessageSubscriber.Application.Features.Queries.DataPoints
{
    public class GetDataPointListViewModel
    {
        public IEnumerable<GetDataPointPaginationViewModel> GetDataPointPaginationViewModels { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class GetDataPointPaginationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
