using Microsoft.EntityFrameworkCore;
using WebAppDemo.Models;

namespace WebAppDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAttribute>()
                .HasKey(pa => pa.AttributeID);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Role)
                .WithMany()
                .HasForeignKey(c => c.RoleID);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin", Description = "System Administrator" },
                new Role { RoleID = 2, RoleName = "Writer", Description = "Content Writer" },
                new Role { RoleID = 3, RoleName = "Member", Description = "General Member" },
                new Role { RoleID = 4, RoleName = "Moderator", Description = "Content Moderator" }
            );
        }
    }
}
