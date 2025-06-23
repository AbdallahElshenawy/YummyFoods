using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Stripe;
using YummyFoods.Components;
using YummyFoods.Components.Account;
using YummyFoods.Data;
using YummyFoods.Repository;
using YummyFoods.Repository.IRepository;
using YummyFoods.Services;

var builder = WebApplication.CreateBuilder(args);
string stripeSecretKey = builder.Configuration["Stripe:SecretKey"];
StripeConfiguration.ApiKey = stripeSecretKey;
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository,ShoppingCartRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddRadzenComponents();
builder.Services.AddSingleton<SharedStateService>();
builder.Services.AddScoped<PaymentService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
     .AddFacebook(options =>
     {
         options.AppId = "1160092848406864";
         options.AppSecret = "a5313a1337e6d9f137b37ed2a4f7cc45";
     })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = "f2cc6e3d-dd2d-4faa-96ac-f823e9ad8ad3";
        options.ClientSecret = "h4r8Q~.1Lu~T7G~hyu2IM-INw9JO8qiXuLsaiasX";
    })
    .AddGoogle(options =>
    {
        options.ClientId = "166243656096-u9jbr6vkb7nfvl1qrpe2ql8debo7dqbg.apps.googleusercontent.com";
        options.ClientSecret = "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";
    })
.AddIdentityCookies();
builder.Services.AddAuthorization();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
