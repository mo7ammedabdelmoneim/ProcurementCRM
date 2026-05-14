using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Persistence.Repositories;

public class LeadRepository : GenericRepository<Lead>, ILeadRepository
{
    public LeadRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Lead>> GetByStatusAsync(
        LeadStatus status, CancellationToken ct = default)
        => await _dbSet
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<Lead>> GetAssignedToUserAsync(
        Guid userId, CancellationToken ct = default)
        => await _dbSet
            .Where(l => l.AssignedToUserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);
}
