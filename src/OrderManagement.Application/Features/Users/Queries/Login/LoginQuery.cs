using MediatR;

namespace OrderManagement.Application.Features.Users.Queries.Login;

public sealed record LoginQuery(LoginRequest Request) : IRequest<AuthResponse>;
