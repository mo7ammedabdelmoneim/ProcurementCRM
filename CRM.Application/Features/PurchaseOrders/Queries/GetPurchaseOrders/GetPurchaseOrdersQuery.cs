using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using MediatR;

namespace CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders;

public record GetPurchaseOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    string? StatusFilter = null
) : IRequest<Result<PaginatedResult<PurchaseOrderDto>>>;