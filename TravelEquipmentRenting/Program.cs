using Microsoft.EntityFrameworkCore;

using TravelEquipmentRenting.Infrastructure.Data;
using TravelEquipmentRenting.Infrastructure.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedAccount");
    options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedEmail");
    options.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedPhoneNumber");
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:RequiredLength");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:RequireNonAlphanumeric");
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();