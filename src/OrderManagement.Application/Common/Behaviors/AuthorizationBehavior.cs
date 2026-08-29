using MediatR;
using OrderManagement.Domain;

namespace OrderManagement.Application;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUser currentUser) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IAuthenticatedRequest)
        {
            return await next();
        }

        currentUser.GetRequiredUserId();

        if (request is IAdminRequest && currentUser.Role != Roles.Admin)
        {
            throw new AppException(403, "forbidden", "You do not have permission to perform this operation.");
        }

        if (request is ICustomerRequest && currentUser.Role != Roles.Customer)
        {
            throw new AppException(403, "forbidden", "You do not have permission to perform this operation.");
        }

        return await next();
    }
}
