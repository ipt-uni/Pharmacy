using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data.Models;

namespace pharmacy.Data;

/// <summary>
/// The application's database context handling all entity mappings.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    /// <summary>
    /// Base user profile data shared by both Customer and Staff.
    /// </summary>
    public DbSet<MyUser> MyUsers { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<NonCustomer> NonCustomers { get; set; } = null!;
}
