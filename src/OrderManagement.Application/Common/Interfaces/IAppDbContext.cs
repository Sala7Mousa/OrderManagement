using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;

namespace OrderManagement.Application;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
