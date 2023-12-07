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
                // Look for any products
                if (context.Product.Any())
                {
                    return;   // DB has been seeded
                }
                context.Product.AddRange(
                    new Product
                    {
                        Title = "Sunday",
                        Genre = "Alternative",
                        Performer = "The Cranberries",
                        Price = 7.99m,
                        Quantity = 5
                    },
                    new Product
                    {
                        Title = "Zero",
                        Genre = "Indie",
                        Performer = "The Smashing Pumpkins",
                        Price = 12.34m,
                        Quantity = 99
                    },
                    new Product
                    {
                        Title = "Blind",
                        Genre = "Metal",
                        Performer = "The Smashing Pumpkins",
                        Price = 8.38m,
                        Quantity = 99
                    },
                    new Product
                    {
                        Title = "Twilight",
                        Genre = "Alternative",
                        Performer = "Bôa",
                        Price = 10.00m,
                        Quantity = 99
                    },
                    new Product
                    {
                        Title = "Please, Please, Please, Let Me Get What I Want",
                        Genre = "Alternative",
                        Performer = "The Smiths",
                        Price = 19.84m,
                        Quantity = 99
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
