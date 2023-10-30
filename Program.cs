using eCourier.Data;
using eCourier.Extention;
using eCourier.Helper;
using eCourier.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<AppUser>()
   .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<AppRole>>()
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


app.Use(async (context, next) =>
{

    var principal = context.User;
    var userManager = context.RequestServices.GetRequiredService<UserManager<AppUser>>();

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

app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
