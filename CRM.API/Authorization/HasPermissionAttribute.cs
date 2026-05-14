using Microsoft.AspNetCore.Authorization;

namespace CRM.API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base($"Permission:{permission}") { }
}
