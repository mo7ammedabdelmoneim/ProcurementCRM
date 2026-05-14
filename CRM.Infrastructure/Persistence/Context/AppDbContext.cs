using CRM.Domain.Common;
using CRM.Domain.Entities;
using CRM.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CRM.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher)
        : base(options) => _publisher = publisher;

    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-apply all IEntityTypeConfiguration classes in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Auto-set UpdatedAt on every modified BaseEntity
        foreach (var entry in ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified))
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        var result = await base.SaveChangesAsync(ct);

        // Publish domain events AFTER saving — so DB is consistent first
        await PublishDomainEventsAsync(ct);

        return result;
    }

    private async Task PublishDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity is Lead || e.Entity is PurchaseOrder)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .OfType<Lead>().SelectMany(l => l.DomainEvents).Cast<IDomainEvent>()
            .Concat(entities.OfType<PurchaseOrder>().SelectMany(p => p.DomainEvents))
            .ToList();

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, ct);

        entities.OfType<Lead>().ToList().ForEach(l => l.ClearDomainEvents());
        entities.OfType<PurchaseOrder>().ToList().ForEach(p => p.ClearDomainEvents());
    }
}