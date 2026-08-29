using System.Security.Claims;
using OrderManagement.Application;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public Guid? Id => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? accessor.HttpContext?.User.FindFirstValue("sub"), out var id) ? id : null;
    public string? Role => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
}
