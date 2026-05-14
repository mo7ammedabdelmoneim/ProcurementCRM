using CRM.Domain.Common;

namespace CRM.Domain.Entities;
public class Contact : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Company { get; private set; }
    public string? JobTitle { get; private set; }
    public string? Notes { get; private set; }

    // Computed full name 
    public string FullName => $"{FirstName} {LastName}";

    private Contact() { }

    public Contact(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void Update(string firstName, string lastName, string? phone,
        string? company, string? jobTitle, string? notes)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Company = company;
        JobTitle = jobTitle;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}