using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using AutoMapper;
using MediatR;
using CRM.Application.Common.Results;
using CRM.Domain.Enums;

namespace CRM.Application.Features.Leads.Queries.GetLeads;

public class GetLeadsQueryHandler
    : IRequestHandler<GetLeadsQuery, Result<PaginatedResult<LeadDto>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLeadsQueryHandler(IUnitOfWork uow, IMapper mapper)
        => (_uow, _mapper) = (uow, mapper);

    public async Task<Result<PaginatedResult<LeadDto>>> Handle(
        GetLeadsQuery request, CancellationToken ct)
    {
        var leads = await _uow.Leads.GetAllAsync(ct);

        if (!string.IsNullOrEmpty(request.StatusFilter) &&
            Enum.TryParse<LeadStatus>(request.StatusFilter, out var status))
        {
            leads = leads.Where(l => l.Status == status);
        }

        var totalCount = leads.Count();
        var items = leads
            .OrderByDescending(l => l.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<LeadDto>>(items);

        return Result.Success(new PaginatedResult<LeadDto>(
            dtos, totalCount, request.Page, request.PageSize));
    }
}