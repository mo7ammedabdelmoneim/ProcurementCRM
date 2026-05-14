namespace CRM.Application.DTOs;

public record PurchaseOrderDto(
    Guid Id,
    string Title,
    decimal TotalAmount,
    string Status,
    Guid RequestedByUserId,
    Guid? ApprovedByUserId,
    string? RejectionReason,
    List<PurchaseOrderItemDto> Items,
    DateTime CreatedAt
);
