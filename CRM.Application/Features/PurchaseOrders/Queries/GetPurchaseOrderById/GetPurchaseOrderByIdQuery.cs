using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using MediatR;

namespace CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public record GetPurchaseOrderByIdQuery(Guid Id)
    : IRequest<Result<PurchaseOrderDto>>;
