using AutoMapper;
using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using CRM.Domain.Enums;
using CRM.Domain.Interfaces;
using MediatR;

namespace CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders;

public class GetPurchaseOrdersQueryHandler
    : IRequestHandler<GetPurchaseOrdersQuery, Result<PaginatedResult<PurchaseOrderDto>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPurchaseOrdersQueryHandler(IUnitOfWork uow, IMapper mapper)
        => (_uow, _mapper) = (uow, mapper);

    public async Task<Result<PaginatedResult<PurchaseOrderDto>>> Handle(
        GetPurchaseOrdersQuery request, CancellationToken ct)
    {
        var orders = await _uow.PurchaseOrders.GetAllAsync(ct);

        if (!string.IsNullOrEmpty(request.StatusFilter) &&
            Enum.TryParse<PurchaseOrderStatus>(request.StatusFilter, out var status))
        {
            orders = orders.Where(po => po.Status == status);
        }

        var totalCount = orders.Count();
        var items = orders
            .OrderByDescending(po => po.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<PurchaseOrderDto>>(items);

        return Result.Success(new PaginatedResult<PurchaseOrderDto>(
            dtos, totalCount, request.Page, request.PageSize));
    }
}