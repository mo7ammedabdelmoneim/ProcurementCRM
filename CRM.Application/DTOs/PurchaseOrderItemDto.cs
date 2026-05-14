namespace CRM.Application.DTOs;

public record PurchaseOrderItemDto(
    Guid Id,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
