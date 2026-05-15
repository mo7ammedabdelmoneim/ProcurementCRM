using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;
<<<<<<< HEAD
=======

namespace CRM.Domain.Entities;

>>>>>>> b80a905c9c1d6b724a8dcb8ee2dd9f97a92ce30d
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Granular permissions on top of role
    private readonly List<string> _permissions = [];
    public IReadOnlyList<string> Permissions => _permissions.AsReadOnly();

    public string FullName => $"{FirstName} {LastName}";

    private User() { }

    public User(string firstName, string lastName, string email,
        string passwordHash, UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void GrantPermission(string permission)
    {
        if (!_permissions.Contains(permission))
            _permissions.Add(permission);
    }

    public void RevokePermission(string permission)
        => _permissions.Remove(permission);

    public void ChangeRole(UserRole newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}