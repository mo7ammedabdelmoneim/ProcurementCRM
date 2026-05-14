using CRM.Domain.Entities;
using CRM.Domain.Enums;

namespace CRM.Domain.Interfaces;

public interface ILeadRepository : IRepository<Lead>
{
    Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status, CancellationToken ct = default);
    Task<IEnumerable<Lead>> GetAssignedToUserAsync(Guid userId, CancellationToken ct = default);
}
