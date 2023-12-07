using Microsoft.EntityFrameworkCore;
using MusicShop.Data;

namespace MusicShop.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MusicShopContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MusicShopContext>>()))
            {
                // Look for any movies.
                if (context.Product.Any())
                {
                    return;   // DB has been seeded
                }
                context.Product.AddRange(
                    new Product
                    {
                        Title = "Sunday",
                        Genre = "Alternative Rock",
                        Performer = "The Cranberries",
                        Price = 7.99m,
                        Quantity = 5
                    },
                    new Product
                    {
                        Title = "Zero",
                        Genre = "Indie Rock",
                        Performer = "The Smashing Pumpkins",
                        Price = 12.34m,
                        Quantity = 99
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
