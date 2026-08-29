namespace OrderManagement.Application;

public interface ICurrentUser { Guid? Id { get; } string? Role { get; } }
