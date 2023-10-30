using eCourier.Data;
using eCourier.Extention;
using eCourier.Helper;
using eCourier.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<IdentityUser>(opt => opt.SignIn.RequireConfirmedAccount = true)
  .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddDependencies();

builder.Services.AddSingleton<ISystemClock, SystemClock>();


 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAutoMapper(typeof(AutomapperProfile).Assembly);

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });

    option.AddPolicy("CustomerOnly", policy =>
    {
        policy.RequireRole("Customer");
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


 

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Use(async (context, next) =>
{      
    
    var principal = context.User;
    var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

    var user = await userManager.GetUserAsync(principal);

    if (user is not null)
    {
        var inRoles = await userManager.GetRolesAsync(user);

        if (inRoles is null || !inRoles.Any())
        {
            await userManager.AddToRoleAsync(user, "Customer");
        }
    }

    var isInCustomerRole = context.User.IsInRole("Customer");

    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
