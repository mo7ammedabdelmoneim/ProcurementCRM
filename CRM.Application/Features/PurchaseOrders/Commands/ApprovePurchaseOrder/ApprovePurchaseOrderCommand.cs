using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using MediatR;
using CRM.Application.Common.Results;

namespace CRM.Application.Features.PurchaseOrders.Commands.ApprovePurchaseOrder;

public record ApprovePurchaseOrderCommand(Guid PurchaseOrderId)
    : IRequest<Result<PurchaseOrderDto>>;