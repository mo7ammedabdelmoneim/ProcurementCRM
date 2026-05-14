using CRM.Domain.Common;
using CRM.Domain.Enums;
using CRM.Domain.Events;
using CRM.Domain.Exceptions;

namespace CRM.Domain.Entities;

public class Lead : BaseEntity
{
    // Private setters — state changes only through domain methods
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public LeadStatus Status { get; private set; }
    public LeadSource Source { get; private set; }
    public string? Notes { get; private set; }
    public Guid AssignedToUserId { get; private set; }

    // Domain events — raised internally, published by Infrastructure
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    // For EF Core 
    private Lead() { }

    public Lead(string name, string email, LeadSource source, Guid assignedToUserId)
    {
        Name = name;
        Email = email;
        Source = source;
        AssignedToUserId = assignedToUserId;
        Status = LeadStatus.New;

        _domainEvents.Add(new LeadCreatedEvent(Id, name, email));
    }

    // Domain method 
    public void Qualify()
    {
        if (Status != LeadStatus.New && Status != LeadStatus.Contacted)
            throw new DomainException($"Cannot qualify a lead with status {Status}.");

        Status = LeadStatus.Qualified;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsWon()
    {
        if (Status != LeadStatus.Qualified)
            throw new DomainException($"Only qualified leads can be marked as Won.");

        Status = LeadStatus.Won;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new LeadWonEvent(Id, AssignedToUserId)); 
    }

    public void MarkAsLost(string reason)
    {
        if (Status == LeadStatus.Won)
            throw new DomainException("Cannot mark a won lead as lost.");

        Status = LeadStatus.Lost;
        Notes = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reassign(Guid newUserId)
    {
        AssignedToUserId = newUserId;
        UpdatedAt = DateTime.UtcNow;
    }
}