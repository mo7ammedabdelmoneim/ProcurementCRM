using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Persistence.Repositories;

public class PurchaseOrderRepository
    : GenericRepository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync(
        CancellationToken ct = default)
        => await _dbSet
            .Include(po => po.Items)
            .Where(po => po.Status == PurchaseOrderStatus.Submitted)
            .OrderBy(po => po.CreatedAt)
            .ToListAsync(ct);
}