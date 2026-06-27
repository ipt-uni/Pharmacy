using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pharmacy.Pages;

[Authorize(Roles = "Staff")]
public class ChathubModel : PageModel
{
    public readonly UserManager<IdentityUser> _userManager;

    public ChathubModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
    }
}
