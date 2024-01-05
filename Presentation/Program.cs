using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Entity.Models;
using DataAccess.UnitOfWorld;
using DataAccess.Interface;
using DataAccess.Repository;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using AutoMapper;
using Entity.Common;
using Stripe;
using System.Text.Json.Serialization;
using System.Net.NetworkInformation;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

}).AddCookie()
  .AddGoogle(GoogleDefaults.AuthenticationScheme, option =>
  {
      option.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
      option.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecrect").Value;
  });
builder.Services.AddControllersWithViews().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookWebStoreDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<Entity.Models.Customer, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<BookWebStoreDbContext>().AddDefaultTokenProviders().AddDefaultUI();
//
builder.Services.AddTransient<IServiceProvider, ServiceProvider>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<ICategoryService,CategoryService>();
builder.Services.AddTransient<ICartItemService,CartItemService>();
builder.Services.AddTransient<ICartService,CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IPublisherService, PublisherService>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
/*builder.Services.AddDefaultIdentity<Customer>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<BookWebStoreDbContext>();*/
//builder.Services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<BookWebStoreDbContext>();
var app = builder.Build();
//
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedDefaulData(scope.ServiceProvider);

}
//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseAuthorization();
app.UseSession();
app.MapAreaControllerRoute(
    name: "User",
    areaName: "User",
    pattern: "User/{controller=Home}/{action=Index}/{id?}"
);
app.MapAreaControllerRoute(
    name: "Librarian",
    areaName: "Librarian",
    pattern: "Librarian/{controller=Home}/{action=Index}/{id?}"
);
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
