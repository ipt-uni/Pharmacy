using Microsoft.AspNetCore.Identity;
using pharmacy.Data;

namespace pharmacy.Data.Seed;

public static class DbInitializerExtensions
{
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            DbInitializer.Seed(context, userManager, roleManager).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            // log ex
        }

        return app;
    }
}
