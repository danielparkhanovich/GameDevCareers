using JobBoardPlatform.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    }
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.EmployeeOnlyPolicy, policy => policy.RequireRole(UserRoles.Employee));
    options.AddPolicy(AuthorizationPolicies.CompanyOnlyPolicy, policy => policy.RequireRole(UserRoles.Company));
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("JobBoardPlatform.DAL"));
});

builder.Services.AddTransient(typeof(IRepository<>), typeof(CoreRepository<>));
builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));

if (!builder.Environment.IsDevelopment()) 
{
    builder.Services.AddTransient<IOfferActionHandlerFactory, OfferActionHandlerFactory>();
}
else 
{
    builder.Services.AddTransient<IOfferActionHandlerFactory, OfferActionEmptyHandlerFactory>();
}
//

var app = builder.Build();

if (!app.Environment.IsDevelopment())
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

app.Run();
