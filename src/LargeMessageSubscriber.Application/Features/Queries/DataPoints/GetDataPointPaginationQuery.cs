using MediatR;

namespace LargeMessageSubscriber.Application.Features.Queries.DataPoints
{
    public class GetDataPointPaginationQuery : IRequest<GetDataPointListViewModel>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
