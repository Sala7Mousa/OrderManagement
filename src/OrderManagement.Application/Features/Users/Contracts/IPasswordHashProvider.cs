namespace OrderManagement.Application;

public interface IPasswordHashProvider
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
