using MediatR;

namespace OrderManagement.Application.Features.Users.Commands.Register;

public sealed record RegisterCommand(RegisterRequest Request) : IRequest<AuthResponse>;
