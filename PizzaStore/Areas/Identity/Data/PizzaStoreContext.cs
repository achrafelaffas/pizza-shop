using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Models;

namespace PizzaStore.Data;

public class PizzaStoreContext : IdentityDbContext<PizzaStoreUser>
{
    public PizzaStoreContext(DbContextOptions<PizzaStoreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }


    public DbSet<Order> orders { get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<Details> details { get; set; }
}
