using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PizzaStoreContextConnection") ?? throw new InvalidOperationException("Connection string 'PizzaStoreContextConnection' not found.");


builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<PizzaStoreUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<PizzaStoreContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Set the login page path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied page path
    });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "YourCartCookieName";
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();;
app.UseAuthorization();

app.MapRazorPages();
app.UseSession();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
