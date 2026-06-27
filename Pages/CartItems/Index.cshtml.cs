using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.CartItems;

[Authorize]
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

    public async Task<IActionResult> OnPostAddToCart(int medicineId)
    {
        var userId = _userManager.GetUserId(User);

        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Customer.UserId == userId && c.Payment == null);

        if (cart == null)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            cart = new Cart { Customer = customer };
            _context.Carts.Add(cart);
        }

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

    public async Task<IActionResult> OnPostIncrease(int cartItemId)
    {
        // in here the cart can't be null

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

    public async Task<IActionResult> OnPostDecrease(int cartItemId)
    {
        var userId = _userManager.GetUserId(User);

        var cartItem = await _context
            .CartItems.Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart!.Customer.UserId == userId);

        if (cartItem == null)
            return NotFound();

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
}
