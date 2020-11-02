using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data.SeedData
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole
                {
                    Name = "Admin"
                },
                new AppRole
                {
                    Name = "Moderator"
                },
                new AppRole
                {
                    Name = "Member"
                }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Password123^");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };
            await userManager.CreateAsync(admin, "Password123^");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
            
        }

        public static async Task SeedCategories(DataContext context)
        {
            if (await context.Categories.AnyAsync()) return;
            {
                var categoryData = System.IO.File.ReadAllText("Data/SeedData/CategoriesSeedData.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);
                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }
                context.SaveChanges();
            }
        }

        public static async Task SeedKitchenOrigins(DataContext context)
        {
            if (await context.KitchenOrigins.AnyAsync()) return;
            {
                var kitchenOriginData = System.IO.File.ReadAllText("Data/SeedData/KitchenOriginsSeedData.json");
                var kitchenOrigins = JsonSerializer.Deserialize<List<KitchenOrigin>>(kitchenOriginData);
                foreach (var origin in kitchenOrigins)
                {
                    context.KitchenOrigins.Add(origin);
                }
                context.SaveChanges();
            }
        }
        // TODO 
        // 1. Recipes
        // 2. Categories
        // 3. Kitchen region (Europa, Asia, Africa, America) every region must contain country. E.g. Europa, Country: France
    }
}