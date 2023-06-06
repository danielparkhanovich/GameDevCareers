using JobBoardPlatform.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;
using JobBoardPlatform.BLL.Services.Background;
using JobBoardPlatform.PL.Requirements;
using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Commands.Admin;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

// Authorization/Authentication
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
    options.AddPolicy(AuthorizationPolicies.EmployeeOnlyPolicy, policy => policy.RequireRole(UserRoles.Employee, UserRoles.Admin));
    options.AddPolicy(AuthorizationPolicies.CompanyOnlyPolicy, policy => policy.RequireRole(UserRoles.Company, UserRoles.Admin));
    options.AddPolicy(AuthorizationPolicies.AdminOnlyPolicy, policy => policy.RequireRole(UserRoles.Admin));
    options.AddPolicy(AuthorizationPolicies.OfferOwnerOnlyPolicy, policy =>
    {
        policy.RequireRole(UserRoles.Company, UserRoles.Admin);
        policy.AddRequirements(new OfferOwnerOrAdminRequirement("offerId"));
    });
    options.AddPolicy(AuthorizationPolicies.OfferPublishedOrOwnerOnlyPolicy, policy =>
    {
        policy.AddRequirements(new OfferPublishedOrOwnerOrAdminRequirement("id"));
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthorizationHandler, OfferOwnerHandler>();
builder.Services.AddTransient<IAuthorizationHandler, OfferPublishedOrOwnerHandler>();

// BLL
builder.Services.AddTransient<OffersCacheManager>();
builder.Services.AddTransient<OfferCommandsExecutor>();
builder.Services.AddTransient<AdminCommandsExecutor>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelApplicationSearchParameters>, CompanyPanelApplicationSearchParametersFactory>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelOfferSearchParameters>, CompanyPanelOfferSearchParametersFactory>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<MainPageOfferSearchParams>, MainPageOfferSearchParamsFactory>();

builder.Services.AddTransient<OfferApplicationsSearcher>();
builder.Services.AddTransient<CompanyOffersSearcher>();
builder.Services.AddTransient<MainPageOffersSearcher>();
builder.Services.AddTransient<MainPageOffersSearcherCacheDecorator>();

// DAL
// Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString");
    options.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName");
});
builder.Services.AddTransient<ICacheRepository<List<JobOffer>>, MainPageOffersCacheRepository>();
builder.Services.AddTransient<ICacheRepository<int>, MainPageOffersCountCacheRepository>();
// Repository
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("JobBoardPlatform.DAL"));
});
builder.Services.AddTransient(typeof(IRepository<>), typeof(CoreRepository<>));
// Cloud
builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));

// Background
builder.Services.AddHostedService<OfferExpirationChecker>();

// Actions
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
