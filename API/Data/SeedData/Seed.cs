using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.SeedData
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
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