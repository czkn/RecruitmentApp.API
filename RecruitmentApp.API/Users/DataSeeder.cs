using System.Security.Cryptography;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentApp.API.Users;

public class DataSeeder
{
    public async Task SeedData(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        Env.Load();
        
        var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        await roleManager.CreateAsync(new IdentityRole<Guid>("Hr"));
        await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

        var admin = new User { Email = adminEmail, UserName = adminEmail, FirstName = "Admin", LastName = "Admin",
            SecurityStamp = GenerateRandomSecurityStamp(), CompanyName = "ADMIN"};
        var hasher = new PasswordHasher<User>();
        admin.PasswordHash = hasher.HashPassword(admin, adminPassword);
        await userManager.CreateAsync(admin);
        await userManager.AddToRoleAsync(admin, "Admin");
    }
    
    private string GenerateRandomSecurityStamp()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}