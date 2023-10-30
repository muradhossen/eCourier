using eCourier.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCourier.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(c => c.Recipient)
                .WithMany(c => c.RecipientOrders)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.AppUser)
                .WithMany(c => c.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>()
              .HasMany(ur => ur.UserRoles)
              .WithOne(u => u.User)
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            builder.Entity<AppRole>()
                .HasData(
                new AppRole { Id = 1, Name = "Customer", NormalizedName = "CUSTOMER"},
                new AppRole { Id = 2 , Name = "Admin", NormalizedName = "Admin"});
        }
    }
}