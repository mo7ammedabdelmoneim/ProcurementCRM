using CRM.API.Authorization;
using CRM.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using CRM.Application.Features.PurchaseOrders.Commands.SubmitPurchaseOrder;
using CRM.Application.Features.PurchaseOrders.Commands.ApprovePurchaseOrder;
using CRM.Application.Features.PurchaseOrders.Commands.RejectPurchaseOrder;
using CRM.Application.Features.PurchaseOrders.Commands.ReceivePurchaseOrder;
using CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders;
using CRM.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers;

[ApiController]
[Route("api/purchase-orders")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly ISender _sender;
    public PurchaseOrdersController(ISender sender) => _sender = sender;

    [HttpGet]
    [HasPermission("po.read")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(
            new GetPurchaseOrdersQuery(page, pageSize, status), ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [HasPermission("po.read")]
    public async Task<IActionResult> GetById(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetPurchaseOrderByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    [HasPermission("po.create")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePurchaseOrderCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Error);
    }

    // ── Workflow transitions ───────────────────────────────────

    [HttpPatch("{id:guid}/submit")]
    [HasPermission("po.create")]
    public async Task<IActionResult> Submit(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new SubmitPurchaseOrderCommand(id), ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpPatch("{id:guid}/approve")]
    [HasPermission("po.approve")]   // Only Manager/Admin
    public async Task<IActionResult> Approve(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new ApprovePurchaseOrderCommand(id), ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpPatch("{id:guid}/reject")]
    [HasPermission("po.approve")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectPurchaseOrderCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command with { PurchaseOrderId = id }, ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }

    [HttpPatch("{id:guid}/receive")]
    [HasPermission("po.receive")]
    public async Task<IActionResult> Receive(
        Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new ReceivePurchaseOrderCommand(id), ct);
        return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity(result.Error);
    }
}