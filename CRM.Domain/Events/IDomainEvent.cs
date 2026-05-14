namespace CRM.Domain.Events;

public interface IDomainEvent { }

// Lead events
public record LeadCreatedEvent(Guid LeadId, string Name, string Email) : IDomainEvent;
public record LeadWonEvent(Guid LeadId, Guid AssignedToUserId) : IDomainEvent;

// Purchase Order events
public record PurchaseOrderSubmittedEvent(Guid OrderId, Guid RequestedByUserId, decimal TotalAmount) : IDomainEvent;
public record PurchaseOrderApprovedEvent(Guid OrderId, Guid ApprovedByUserId) : IDomainEvent;
public record PurchaseOrderRejectedEvent(Guid OrderId, Guid RejectedByUserId, string Reason) : IDomainEvent;
public record PurchaseOrderReceivedEvent(Guid OrderId) : IDomainEvent;
public record PurchaseOrderInvoicedEvent(Guid OrderId) : IDomainEvent;