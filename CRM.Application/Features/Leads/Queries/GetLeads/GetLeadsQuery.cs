using CRM.Application.Common;
using CRM.Application.DTOs;
using MediatR;
using CRM.Application.Common.Results;

namespace CRM.Application.Features.Leads.Queries.GetLeads;

public record GetLeadsQuery(
    int Page = 1,
    int PageSize = 20,
    string? StatusFilter = null
) : IRequest<Result<PaginatedResult<LeadDto>>>;
