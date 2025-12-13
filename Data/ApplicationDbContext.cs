using DeckMaster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeckMaster.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Configures entity models and seed data.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Identity Roles
            SeedIdentityRoles(modelBuilder);

            // Seed Products
            SeedProducts(modelBuilder);
        }

        /// <summary>
        /// Seeds default identity roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        private void SeedIdentityRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "Ad", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "Ma", Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "Cu", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }

        /// <summary>
        /// Seeds default product data.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    ProductName = "Red Cards",
                    Description = "Are you looking for a fun and affordable way to pass " +
                                    "the time? Look no further than our high-quality deck " +
                                    "of cards! At only $3.79, this deck of cards is an " +
                                    "excellent value.",
                    Price = "3.79",
                    Currency = "CAD",
                    Image = "DeckOfCards.png"
                },
                new Product
                {
                    ID = 2,
                    ProductName = "Extra Ace",
                    Description = "Are you tired of being caught without an ace up your " +
                                    "sleeve? Well, have no fear! Our special deck of cards " +
                                    "comes with an extra ace, so you can always have the " +
                                    "upper hand. And at just $4.95, it's a steal!",
                    Price = "4.95",
                    Currency = "CAD",
                    Image = "FiveAces.jpg"
                },
                new Product
                {
                    ID = 3,
                    ProductName = "Black Deck",
                    Description = "Upgrade your card game with our premium black-styled " +
                                    "deck of cards. Made with high-quality materials and " +
                                    "featuring a sleek black design. At just $7.79, it's " +
                                    "a small price to pay to make a big statement!",
                    Price = "7.79",
                    Currency = "CAD",
                    Image = "BlackDeck.jpeg"
                });
        }
    }
}
