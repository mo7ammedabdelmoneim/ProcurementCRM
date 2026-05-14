using CRM.Domain.Common;
using CRM.Domain.Enums;
using CRM.Domain.Events;
using CRM.Domain.Exceptions;

namespace CRM.Domain.Entities;

public class PurchaseOrder : BaseEntity
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal TotalAmount { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public Guid RequestedByUserId { get; private set; }
    public Guid? ApprovedByUserId { get; private set; }
    public string? RejectionReason { get; private set; }
    public Guid? SupplierId { get; private set; }

    // Owned collection of line items
    private readonly List<PurchaseOrderItem> _items = [];
    public IReadOnlyList<PurchaseOrderItem> Items => _items.AsReadOnly();

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    private PurchaseOrder() { }

    public PurchaseOrder(string title, decimal totalAmount, Guid requestedByUserId, Guid? supplierId = null)
    {
        Title = title;
        TotalAmount = totalAmount;
        RequestedByUserId = requestedByUserId;
        SupplierId = supplierId;
        Status = PurchaseOrderStatus.Draft;
    }

    public void AddItem(string description, int quantity, decimal unitPrice)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new DomainException("Items can only be added to a draft purchase order.");

        _items.Add(new PurchaseOrderItem(Id, description, quantity, unitPrice));
        TotalAmount = _items.Sum(i => i.Quantity * i.UnitPrice);
    }

    public void Submit()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new DomainException("Only draft purchase orders can be submitted.");
        if (!_items.Any())
            throw new DomainException("Cannot submit a purchase order with no items.");

        Status = PurchaseOrderStatus.Submitted;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PurchaseOrderSubmittedEvent(Id, RequestedByUserId, TotalAmount));
    }

    public void Approve(Guid approvedByUserId)
    {
        if (Status != PurchaseOrderStatus.Submitted)
            throw new DomainException("Only submitted purchase orders can be approved.");

        Status = PurchaseOrderStatus.Approved;
        ApprovedByUserId = approvedByUserId;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PurchaseOrderApprovedEvent(Id, approvedByUserId));
    }

    public void Reject(Guid rejectedByUserId, string reason)
    {
        if (Status != PurchaseOrderStatus.Submitted)
            throw new DomainException("Only submitted purchase orders can be rejected.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Rejection reason is required.");

        Status = PurchaseOrderStatus.Rejected;
        RejectionReason = reason;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PurchaseOrderRejectedEvent(Id, rejectedByUserId, reason));
    }

    public void MarkAsReceived()
    {
        if (Status != PurchaseOrderStatus.Approved)
            throw new DomainException("Only approved purchase orders can be marked as received.");

        Status = PurchaseOrderStatus.Received;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PurchaseOrderReceivedEvent(Id));
    }

    public void MarkAsInvoiced()
    {
        if (Status != PurchaseOrderStatus.Received)
            throw new DomainException("Only received purchase orders can be invoiced.");

        Status = PurchaseOrderStatus.Invoiced;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PurchaseOrderInvoicedEvent(Id));
    }
}