using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data.Models;

namespace pharmacy.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!; DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}
