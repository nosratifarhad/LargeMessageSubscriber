using MediatR;
using LargeMessageSubscriber.Domain.LargeMessage;
using LargeMessageSubscriber.Application.Common;

namespace LargeMessageSubscriber.Application.Features.Queries.DataPoints
{
    public class GetDataPointPaginationQueryHandler :
        IRequestHandler<GetDataPointPaginationQuery, GetDataPointListViewModel>
    {
        private readonly IRepository _repository;

        public GetDataPointPaginationQueryHandler(IRepository repository)
        {
            this._repository = repository;
        }

        public async Task<GetDataPointListViewModel> Handle(GetDataPointPaginationQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return null;

            ValidatePagination(request);

            var dataPoints = await _repository.GetDataPointPagination(request.PageSize, request.PageIndex);
            if (dataPoints == null)
                return null;

            return new GetDataPointListViewModel()
            {
                GetDataPointPaginationViewModels = (IEnumerable<GetDataPointPaginationViewModel>)dataPoints,
                Pagination = new Pagination()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    //TotalCount = dataPoints.TotalCount
                }
            };
        }

        private void ValidatePagination(GetDataPointPaginationQuery request)
        {
            if (request.PageIndex == 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == 0)
            {
                request.PageSize = 10;
            }
        }
    }
}
