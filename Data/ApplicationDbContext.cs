using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data.Models;

namespace pharmacy.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    DbSet<Company> Companies { get; set; } = null!;
    DbSet<Medicine> Medicines { get; set; } = null!; DbSet<Supplier> Suppliers { get; set; } = null!;
    DbSet<Cart> Carts { get; set; } = null!;
    DbSet<CartItem> CartItems { get; set; } = null!;
    DbSet<Payment> Payments { get; set; } = null!;
}
