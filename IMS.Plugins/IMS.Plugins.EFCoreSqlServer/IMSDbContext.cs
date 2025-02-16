using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class IMSDbContext : DbContext
    {
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ProductTransaction> ProductTransactions { get; set; }

        public IMSDbContext(DbContextOptions<IMSDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProductInventory>()
                .HasKey(pi => new { pi.ProductId, pi.InventoryId });

            modelBuilder
                .Entity<ProductInventory>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductInventories)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder
                .Entity<ProductInventory>()
                .HasOne(pi => pi.Inventory)
                .WithMany(i => i.ProductInventories)
                .HasForeignKey(pi => pi.InventoryId);

            // seeding data
            modelBuilder
                .Entity<Inventory>()
                .HasData(
                    new Inventory()
                    {
                        Id = 1,
                        Name = "Bike Seat",
                        Quantity = 10,
                        Price = 2,
                    },
                    new Inventory()
                    {
                        Id = 2,
                        Name = "Bike Body",
                        Quantity = 10,
                        Price = 15,
                    },
                    new Inventory()
                    {
                        Id = 3,
                        Name = "Bike Wheels",
                        Quantity = 20,
                        Price = 8,
                    },
                    new Inventory()
                    {
                        Id = 4,
                        Name = "Bike Pedels",
                        Quantity = 20,
                        Price = 1,
                    }
                );

            modelBuilder
                .Entity<Product>()
                .HasData(
                    new Product()
                    {
                        Id = 1,
                        Name = "Bike",
                        Quantity = 10,
                        Price = 150,
                    },
                    new Product()
                    {
                        Id = 2,
                        Name = "Car",
                        Quantity = 5,
                        Price = 25000,
                    }
                );

            modelBuilder
                .Entity<ProductInventory>()
                .HasData(
                    new ProductInventory
                    {
                        ProductId = 1,
                        InventoryId = 1,
                        InventoryQuantity = 1,
                    }, // seat
                    new ProductInventory
                    {
                        ProductId = 1,
                        InventoryId = 2,
                        InventoryQuantity = 1,
                    }, // body
                    new ProductInventory
                    {
                        ProductId = 1,
                        InventoryId = 3,
                        InventoryQuantity = 2,
                    }, //wheels
                    new ProductInventory
                    {
                        ProductId = 1,
                        InventoryId = 4,
                        InventoryQuantity = 2,
                    } //pedal
                );
        }
    }
}
