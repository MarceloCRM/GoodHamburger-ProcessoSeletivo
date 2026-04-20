using GoodHamburger.Models;
using GoodHamburger.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    Name = "X Burger",
                    Price = 5.00m,
                    Category = ItemCategory.Sandwich
                },
                new Item
                {
                    Id = 2,
                    Name = "X Egg",
                    Price = 4.50m,
                    Category = ItemCategory.Sandwich
                },
                new Item
                {
                    Id = 3,
                    Name = "X Bacon",
                    Price = 7.00m,
                    Category = ItemCategory.Sandwich
                },
                new Item
                {
                    Id = 4,
                    Name = "Batata frita",
                    Price = 2.00m,
                    Category = ItemCategory.Fries
                },
                new Item
                {
                    Id = 5,
                    Name = "Refrigerante",
                    Price = 2.50m,
                    Category = ItemCategory.Drink
                }
            );
        }
    }
}
