using Microsoft.EntityFrameworkCore;
using TechStore.App.Models;
namespace TechStore.App.Data;
public class TechStoreDbContext : DbContext
{
 public DbSet<Category> Categories=>Set<Category>(); public DbSet<Product> Products=>Set<Product>(); public DbSet<Branch> Branches=>Set<Branch>(); public DbSet<Inventory> Inventories=>Set<Inventory>(); public DbSet<Customer> Customers=>Set<Customer>(); public DbSet<Seller> Sellers=>Set<Seller>(); public DbSet<Sale> Sales=>Set<Sale>(); public DbSet<SaleItem> SaleItems=>Set<SaleItem>(); public DbSet<CurrentAccountMovement> CurrentAccountMovements=>Set<CurrentAccountMovement>();
 public TechStoreDbContext(){} public TechStoreDbContext(DbContextOptions<TechStoreDbContext> options):base(options){}
 protected override void OnConfiguring(DbContextOptionsBuilder b){if(!b.IsConfigured)b.UseSqlServer(DatabaseOptions.ConnectionString,x=>x.EnableRetryOnFailure()).LogTo(System.Diagnostics.Debug.WriteLine);}
 protected override void OnModelCreating(ModelBuilder b){
  b.Entity<Category>().HasIndex(x=>x.Name).IsUnique(); b.Entity<Product>().HasIndex(x=>x.Code).IsUnique(); b.Entity<Inventory>().HasIndex(x=>new{x.ProductId,x.BranchId}).IsUnique(); b.Entity<Customer>().HasIndex(x=>x.DocumentNumber).IsUnique().HasFilter("[DocumentNumber] IS NOT NULL"); b.Entity<Seller>().HasIndex(x=>x.DocumentNumber).IsUnique(); b.Entity<Sale>().HasIndex(x=>x.InvoiceNumber).IsUnique();
  b.Entity<Product>().Property(x=>x.Price).HasPrecision(18,2); b.Entity<Customer>().Property(x=>x.DiscountPercentage).HasPrecision(5,2); b.Entity<Customer>().Property(x=>x.CurrentAccountBalance).HasPrecision(18,2); foreach(var p in new[]{nameof(Sale.Subtotal),nameof(Sale.DiscountAmount),nameof(Sale.Total)}) b.Entity<Sale>().Property(p).HasPrecision(18,2);
  b.Entity<Category>().Property(x=>x.Name).HasMaxLength(80).IsRequired(); b.Entity<Product>().Property(x=>x.Code).HasMaxLength(30).IsRequired(); b.Entity<Product>().Property(x=>x.Name).HasMaxLength(120).IsRequired(); b.Entity<Customer>().Property(x=>x.BusinessName).HasMaxLength(150).IsRequired(); b.Entity<Customer>().ToTable(x=>x.HasCheckConstraint("CK_Customers_Discount","DiscountPercentage >= 0 AND DiscountPercentage <= 100")); b.Entity<Inventory>().ToTable(x=>x.HasCheckConstraint("CK_Inventories_Stock","Stock >= 0 AND MinimumStock >= 0")); b.Entity<SaleItem>().ToTable(x=>x.HasCheckConstraint("CK_SaleItems_Quantity","Quantity > 0"));
  b.Entity<Sale>().HasOne(x=>x.Customer).WithMany().OnDelete(DeleteBehavior.Restrict); b.Entity<Sale>().HasOne(x=>x.Branch).WithMany().OnDelete(DeleteBehavior.Restrict); b.Entity<Sale>().HasOne(x=>x.Seller).WithMany().OnDelete(DeleteBehavior.Restrict); b.Entity<CurrentAccountMovement>().HasOne(x=>x.Sale).WithMany().OnDelete(DeleteBehavior.Restrict);
 }
}
