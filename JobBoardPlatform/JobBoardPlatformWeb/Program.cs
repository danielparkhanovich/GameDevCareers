using JobBoardPlatform.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Employee;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using JobBoardPlatform.BLL.Services.Email;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.BLL.Services.AccountManagement;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.BLL.Commands.Identity;

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
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
        options.Scope.Add("profile");
        options.ClaimActions.MapJsonKey("picture", "picture");
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration.GetValue<string>("Authentication:Facebook:ClientId");
        options.AppSecret = builder.Configuration.GetValue<string>("Authentication:Facebook:ClientSecret");
        options.Fields.Add("picture");
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                var identity = (ClaimsIdentity)context.Principal!.Identity!;
                var profileImg = context.User
                    .GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                identity.AddClaim(new Claim("picture", profileImg));
                return Task.CompletedTask;
            }
        };
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Authentication:GitHub:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:GitHub:ClientSecret");
    })
    .AddLinkedIn(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Authentication:LinkedIn:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:LinkedIn:ClientSecret");
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
builder.Services.AddTransient<IEmailEmployeeRegistrationService, EmailEmployeeRegistrationService>();
builder.Services.AddTransient<IEmailCompanyRegistrationService, EmailCompanyRegistrationService>();
builder.Services.AddTransient<IAuthenticationWithProviderService<EmployeeIdentity>, EmployeeAuthenticationWithProviderService>();
builder.Services.AddTransient(typeof(IRegistrationService<>), typeof(RegistrationService<>));
builder.Services.AddTransient<IRegistrationTokensService, RegistrationTokensService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailGateway"));

builder.Services.AddTransient<IPasswordHasher, MD5Hasher>();
builder.Services.AddTransient<IPasswordGenerator, PasswordGenerator>();

builder.Services.AddTransient(typeof(IAuthorizationService<,>), typeof(AuthorizationService<,>));
builder.Services.AddTransient(typeof(IUserSessionService<,>), typeof(UserSessionService<,>));
builder.Services.AddTransient(typeof(ILoginService<,>), typeof(LoginService<,>));
builder.Services.AddTransient(typeof(IModifyIdentityService<>), typeof(ModifyIdentityService<>));

builder.Services.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
builder.Services.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();
builder.Services.AddTransient<UserManager<EmployeeIdentity>>();
builder.Services.AddTransient<UserManager<CompanyIdentity>>();

builder.Services.AddTransient<OffersCacheManager>();
builder.Services.AddTransient<OfferQueryExecutor>();
builder.Services.AddTransient<OfferCommandsExecutor>();
builder.Services.AddTransient<OfferApplicationCommandsExecutor>();
builder.Services.AddTransient<AdminCommandsExecutor>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelApplicationSearchParams>, CompanyPanelApplicationSearchParamsFactory>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelOfferSearchParameters>, CompanyPanelOfferSearchParametersFactory>();
builder.Services.AddTransient<IPageSearchParamsUrlFactory<MainPageOfferSearchParams>, MainPageOfferSearchParamsFactory>();

builder.Services.AddTransient<OfferApplicationsSearcher>();
builder.Services.AddTransient<CompanyOffersSearcher>();
builder.Services.AddTransient<MainPageOffersSearcher>();
builder.Services.AddTransient<MainPageOffersSearcherCacheDecorator>();

// PL Interactors
builder.Services.AddTransient<IRegistrationInteractor<UserRegisterViewModel>, EmailEmployeeRegistrationInteractor>();
builder.Services.AddTransient<IRegistrationInteractor<CompanyRegisterViewModel>, EmailCompanyRegistrationInteractor>();

// DAL
// Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString");
    options.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName");
});
builder.Services.AddTransient<ICacheRepository<List<JobOffer>>, MainPageOffersCacheRepository>();
builder.Services.AddTransient<ICacheRepository<int>, MainPageOffersCountCacheRepository>();
builder.Services.AddTransient<ICacheRepository<RegistrationToken>, RegistrationTokensCacheRepository>();
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
builder.Services.AddCors();

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

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
