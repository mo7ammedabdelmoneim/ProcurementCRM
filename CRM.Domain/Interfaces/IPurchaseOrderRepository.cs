using CRM.Domain.Entities;

namespace CRM.Domain.Interfaces;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
    Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync(CancellationToken ct = default);
}
