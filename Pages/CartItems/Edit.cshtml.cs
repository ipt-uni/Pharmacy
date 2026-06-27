using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.CartItems;

[Authorize(Roles = "Customer")]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public CartItem CartItem { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToPage("/Identity/Account/Login");

        CartItem = await _context
            .CartItems.Include(ci => ci.Medicine)
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.Cart!.Customer.UserId == userId);

        if (CartItem == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToPage("/Identity/Account/Login");

        var cartItem = await _context
            .CartItems.Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.Cart!.Customer.UserId == userId);

        if (cartItem == null)
            return NotFound();

        if (CartItem.Quantity < 1)
        {
            ModelState.AddModelError("CartItem.Quantity", "Quantity must be at least 1.");
            CartItem = await _context
                .CartItems.Include(ci => ci.Medicine)
                .Include(ci => ci.Cart)
                .FirstAsync(ci => ci.Id == id);
            return Page();
        }

        cartItem.Quantity = CartItem.Quantity;
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
