using CRM.Application.DTOs;
using CRM.Domain.Exceptions;
using CRM.Domain.Interfaces;
using AutoMapper;
using MediatR;
using CRM.Application.Common.Results;
using CRM.Application.Interfaces.Services;

namespace CRM.Application.Features.PurchaseOrders.Commands.ApprovePurchaseOrder;

public class ApprovePurchaseOrderCommandHandler
    : IRequestHandler<ApprovePurchaseOrderCommand, Result<PurchaseOrderDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public ApprovePurchaseOrderCommandHandler(
        IUnitOfWork uow, IMapper mapper, ICurrentUserService currentUser)
        => (_uow, _mapper, _currentUser) = (uow, mapper, currentUser);

    public async Task<Result<PurchaseOrderDto>> Handle(
        ApprovePurchaseOrderCommand request, CancellationToken ct)
    {
        var po = await _uow.PurchaseOrders.GetByIdAsync(request.PurchaseOrderId, ct)
            ?? throw new NotFoundException("PurchaseOrder", request.PurchaseOrderId);

        // status must be Submitted
        po.Approve(_currentUser.UserId);

        _uow.PurchaseOrders.Update(po);
        await _uow.SaveChangesAsync(ct);

        // Domain events (POApprovedEvent) are published by Infrastructure
        // after SaveChangesAsync — triggers email notification via Hangfire

        return Result.Success(_mapper.Map<PurchaseOrderDto>(po));
    }
}