using CRM.Domain.Common;

namespace CRM.Domain.Entities;

namespace CRM.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string EntityName { get; private set; }
    public string EntityId { get; private set; }
    public string Action { get; private set; }   // Created | Updated | Deleted
    public string? OldValues { get; private set; }  // JSON snapshot before
    public string? NewValues { get; private set; }  // JSON snapshot after
    public Guid? UserId { get; private set; }
    public string? IpAddress { get; private set; }

    private AuditLog() { }

    public AuditLog(
        string entityName,
        string entityId,
        string action,
        string? oldValues,
        string? newValues,
        Guid? userId,
        string? ipAddress)
    {
        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        OldValues = oldValues;
        NewValues = newValues;
        UserId = userId;
        IpAddress = ipAddress;
    }
}