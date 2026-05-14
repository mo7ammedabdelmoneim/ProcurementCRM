using CRM.API.Authorization;
using CRM.Application.Features.Leads.Commands.CreateLead;
using CRM.Application.Features.Leads.Commands.QualifyLead;
using CRM.Application.Features.Leads.Commands.MarkLeadAsWon;
using CRM.Application.Features.Leads.Commands.MarkLeadAsLost;
using CRM.Application.Features.Leads.Commands.DeleteLead;
using CRM.Application.Features.Leads.Queries.GetLeads;
using CRM.Application.Features.Leads.Queries.GetLeadById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Server;

namespace CRM.API.Controllers;

[ApiController]
[Route("api/leads")]
[Authorize]
public class LeadsController : ControllerBase
{
    private readonly ISender _sender;
    public LeadsController(ISender sender) => _sender = sender;

    [HttpGet]
    [HasPermission("leads.read")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(
            new GetLeadsQuery(page, pageSize, status), ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [HasPermission("leads.read")]
    public async Task<IActionResult> GetById(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetLeadByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    [HasPermission("leads.create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeadCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Error);
    }

    // ── Workflow transitions ───────────────────────────────────

    [HttpPatch("{id:guid}/qualify")]
    [HasPermission("leads.update")]
    public async Task<IActionResult> Qualify(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new QualifyLeadCommand(id), ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpPatch("{id:guid}/won")]
    [HasPermission("leads.update")]
    public async Task<IActionResult> MarkAsWon(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new MarkLeadAsWonCommand(id), ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpPatch("{id:guid}/lost")]
    [HasPermission("leads.update")]
    public async Task<IActionResult> MarkAsLost(
        Guid id,
        [FromBody] MarkLeadAsLostCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command with { LeadId = id }, ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission("leads.delete")]
    public async Task<IActionResult> Delete(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new DeleteLeadCommand(id), ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}