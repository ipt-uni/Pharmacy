using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.CartItems;

/// <summary>
/// Customer cart page — view items, adjust quantities, remove items, and checkout.
/// </summary>
[Authorize(Roles = "Customer")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public Cart? Cart { get; set; }

    /// <summary>
    /// Loads the current customer's unpaid cart with all items and medicine details.
    /// </summary>
    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return;

        Cart = await _context
            .Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Medicine)
            .FirstOrDefaultAsync(c => c.Customer.UserId == userId && c.Payment == null);
    }

    /// <summary>
    /// Adds a medicine to the cart, or increments quantity if already present.
    /// Creates a new cart for the customer if none exists.
    /// </summary>
    public async Task<IActionResult> OnPostAddToCart(int medicineId)
    {
        var userId = _userManager.GetUserId(User);

        // 1. Find the customer's unpaid cart, or create one if it doesn't exist
        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Customer.UserId == userId && c.Payment == null);

        if (cart == null)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            cart = new Cart { Customer = customer };
            _context.Carts.Add(cart);
        }

        // 2. If the medicine is already in the cart, increase quantity; otherwise add a new item
        var existing = cart.CartItems.FirstOrDefault(ci => ci.MedicineId == medicineId);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);
            if (medicine == null)
                return NotFound();

            cart.CartItems.Add(
                new CartItem
                {
                    MedicineId = medicineId,
                    Quantity = 1,
                    UnitPrice = medicine.RetailPrice,
                }
            );
        }

        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    /// <summary>
    /// Increases the quantity of a cart item by 1.
    /// </summary>
    public async Task<IActionResult> OnPostIncrease(int cartItemId)
    {
        var userId = _userManager.GetUserId(User);

        var cartItem = await _context
            .CartItems.Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart!.Customer.UserId == userId);

        if (cartItem == null)
            return NotFound();

        cartItem.Quantity++;
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    /// <summary>
    /// Decreases the quantity of a cart item by 1.
    /// Removes the item entirely if the quantity would drop to zero.
    /// </summary>
    public async Task<IActionResult> OnPostDecrease(int cartItemId)
    {
        var userId = _userManager.GetUserId(User);

        var cartItem = await _context
            .CartItems.Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart!.Customer.UserId == userId);

        if (cartItem == null)
            return NotFound();

        // Remove item if quantity would become 0, otherwise decrement
        if (cartItem.Quantity <= 1)
        {
            _context.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity--;
        }

        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    /// <summary>
    /// Removes a cart item entirely from the cart.
    /// </summary>
    public async Task<IActionResult> OnPostRemove(int cartItemId)
    {
        var userId = _userManager.GetUserId(User);

        var cartItem = await _context
            .CartItems.Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart!.Customer.UserId == userId);

        if (cartItem == null)
            return NotFound();

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    /// <summary>
    /// Creates a Payment for the current cart, marking it as paid.
    /// The cart will no longer appear in the customer's active cart.
    /// </summary>
    public async Task<IActionResult> OnPostCheckout()
    {
        var userId = _userManager.GetUserId(User);

        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Customer.UserId == userId && c.Payment == null);

        if (cart == null || !cart.CartItems.Any())
            return RedirectToPage();

        // Create a payment with the total cost and link it to the cart
        cart.Payment = new Payment { Amount = cart.TotalCost, CartId = cart.Id };
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
