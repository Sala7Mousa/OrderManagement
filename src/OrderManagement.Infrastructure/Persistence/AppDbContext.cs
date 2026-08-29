using Microsoft.EntityFrameworkCore;
using OrderManagement.Application;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).HasMaxLength(320);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Role).HasMaxLength(32);
            e.HasData(DefaultUserSeeds.Admin, DefaultUserSeeds.Customer);
        });
        b.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id); e.Property(x => x.Name).HasMaxLength(200); e.Property(x => x.Sku).HasMaxLength(64); e.HasIndex(x => x.Sku).IsUnique();
            e.Property(x => x.Price).HasPrecision(18, 2); e.Property(x => x.RowVersion).IsRowVersion();
            e.ToTable(t => { t.HasCheckConstraint("CK_Product_Price", "[Price] >= 0"); t.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0"); });
        });
        b.Entity<Order>(e => { e.HasKey(x => x.Id); e.Property(x => x.TotalAmount).HasPrecision(18, 2); e.HasOne(x => x.Customer).WithMany(x => x.Orders).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict); e.HasIndex(x => new { x.CustomerId, x.CreatedAt }); });
        b.Entity<OrderItem>(e => { e.HasKey(x => x.Id); e.Property(x => x.UnitPrice).HasPrecision(18, 2); e.Ignore(x => x.LineTotal); e.HasIndex(x => new { x.OrderId, x.ProductId }).IsUnique(); e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict); });
    }
}
