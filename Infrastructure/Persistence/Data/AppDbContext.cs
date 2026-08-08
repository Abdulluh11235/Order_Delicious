using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Infrastructure.Persistence.Data;

public class AppDbContext: IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Country> Countries  { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantBranch>  RestaurantBranches { get; set; }
    public DbSet<Menu> Menus { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Item>()
            .HasMany(i => i.Categories)
            .WithMany(c => c.Items);
        
    }
}