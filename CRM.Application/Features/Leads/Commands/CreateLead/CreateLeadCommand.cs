using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using CRM.Domain.Enums;
using MediatR;

namespace CRM.Application.Features.Leads.Commands.CreateLead;

public record CreateLeadCommand(
    string Name,
    string Email,
    string? Phone,
    LeadSource Source,
    Guid AssignedToUserId
) : IRequest<Result<LeadDto>>;
