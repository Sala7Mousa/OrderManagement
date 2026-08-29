namespace OrderManagement.Application;

public static class CurrentUserExtensions
{
    public static Guid GetRequiredUserId(this ICurrentUser currentUser)
    {
        return currentUser.Id
            ?? throw new AppException(401, "unauthorized", "Authentication is required.");
    }
}
