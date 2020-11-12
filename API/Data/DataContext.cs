using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser,
      AppRole, 
      int, 
      IdentityUserClaim<int>,
      AppUserRole,
      IdentityUserLogin<int>,
      IdentityRoleClaim<int>,
      IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<KitchenOrigin> KitchenOrigins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FavouriteRecipe> FavouriteRecipes {get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(userRoles => userRoles.UserRoles)
            .WithOne(user => user.User)
            .HasForeignKey(userRoles => userRoles.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(userRoles => userRoles.UserRoles)
            .WithOne(user => user.Role)
            .HasForeignKey(userRoles => userRoles.RoleId)
            .IsRequired();


            builder.Entity<UserLike>()
            .HasKey(k => new {k.SourceUserId, k.LikedUserId});

            builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l =>l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade); 
            // DeleteBehaviour.NoAction if using SQL Server

            builder.Entity<UserLike>()
            .HasOne(s => s.LikedUser)
            .WithMany(l =>l.LikedByUsers)
            .HasForeignKey(s => s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessageReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FavouriteRecipe>()
            .HasKey(k => new {k.UserId, k.RecipeId});
        }

    }
}