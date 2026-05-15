using AutoMapper;
using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using CRM.Domain.Exceptions;
using CRM.Domain.Interfaces;
using MediatR;

namespace CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQueryHandler
    : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPurchaseOrderByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        => (_uow, _mapper) = (uow, mapper);

    public async Task<Result<PurchaseOrderDto>> Handle(
        GetPurchaseOrderByIdQuery request, CancellationToken ct)
    {
        var po = await _uow.PurchaseOrders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("PurchaseOrder", request.Id);

        return Result.Success(_mapper.Map<PurchaseOrderDto>(po));
    }
}