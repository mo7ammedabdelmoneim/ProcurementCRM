using CRM.Domain.Common;

namespace CRM.Domain.Entities;
<<<<<<< HEAD
=======

>>>>>>> b80a905c9c1d6b724a8dcb8ee2dd9f97a92ce30d
public class PurchaseOrderItem : BaseEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    // Computed 
    public decimal TotalPrice => Quantity * UnitPrice;

    private PurchaseOrderItem() { }

    public PurchaseOrderItem(
        Guid purchaseOrderId,
        string description,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.");

        PurchaseOrderId = purchaseOrderId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}