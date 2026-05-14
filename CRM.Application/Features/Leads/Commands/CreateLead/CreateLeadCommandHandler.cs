using AutoMapper;
using CRM.Application.Common.Results;
using CRM.Application.DTOs;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using MediatR;

namespace CRM.Application.Features.Leads.Commands.CreateLead;

public class CreateLeadCommandHandler
    : IRequestHandler<CreateLeadCommand, Result<LeadDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateLeadCommandHandler(IUnitOfWork uow, IMapper mapper)
        => (_uow, _mapper) = (uow, mapper);

    public async Task<Result<LeadDto>> Handle(
        CreateLeadCommand request, CancellationToken ct)
    {
        var lead = new Lead(
            request.Name,
            request.Email,
            request.Source,
            request.AssignedToUserId);

        await _uow.Leads.AddAsync(lead, ct);
        await _uow.SaveChangesAsync(ct);

        return Result.Success(_mapper.Map<LeadDto>(lead));
    }
}