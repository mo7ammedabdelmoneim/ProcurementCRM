namespace CRM.Domain.Interfaces;

public interface IUnitOfWork
{
    ILeadRepository Leads { get; }
    IPurchaseOrderRepository PurchaseOrders { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}