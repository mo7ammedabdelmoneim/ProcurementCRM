using CRM.Domain.Interfaces;
using CRM.Infrastructure.Persistence.Context;
using CRM.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public ILeadRepository Leads { get; }
    public IPurchaseOrderRepository PurchaseOrders { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Leads = new LeadRepository(context);
        PurchaseOrders = new PurchaseOrderRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync()
        => _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction!.CommitAsync();
        await _transaction.DisposeAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction!.RollbackAsync();
        await _transaction.DisposeAsync();
    }
}