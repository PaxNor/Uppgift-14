using Microsoft.AspNetCore.Identity;
using Uppgift_14.Data;
using Uppgift_14.Models;

public class SeedData
{
    private static ApplicationDbContext db = default!;
    private static RoleManager<IdentityRole> roleManager = default!;
    private static UserManager<ApplicationUser> userManager = default!;

    public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string adminPW) {

        if(adminPW == null) {
            throw new Exception("Unable to get password from config");
        }

        // not using db context in this case, useful if seeding other data
        db = context ?? throw new ArgumentNullException(nameof(context));
        // if seeding users randomly, prevents database from growing on each run
        //if (db.Users.Any()) return;

        roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (roleManager == null) {
            throw new ArgumentNullException(nameof(RoleManager<IdentityRole>));
        }

        userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (userManager == null) {
            throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
        }

        var roleName = "Admin";
        var adminEmail = "admin@gym.se";
        var admin = new ApplicationUser();

        // create user
        if(await userManager.FindByEmailAsync(adminEmail) != null) return;

        admin.UserName = adminEmail;
        admin.Email = adminEmail;

        var result = await userManager.CreateAsync(admin, adminPW);
        if (result.Succeeded == false) {
            throw new Exception(string.Join("\n", result.Errors));
        }

        // create role "Admin"
        if (await roleManager.RoleExistsAsync(roleName)) return;

        var role = new IdentityRole { Name = roleName };
        result = await roleManager.CreateAsync(role);
        if (result.Succeeded == false) {
            throw new Exception(string.Join("\n", result.Errors));
        }

        // add role to user
        if (await userManager.IsInRoleAsync(admin, roleName)) return;

        result = await userManager.AddToRoleAsync(admin, roleName);
        if (result.Succeeded == false) {
            throw new Exception(string.Join("\n", result.Errors));
        }
    }
}