using OrderManagement.Domain;

namespace OrderManagement.Application;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
