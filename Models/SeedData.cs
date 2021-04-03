using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

namespace AzureContainer.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetService<ProductDbContext>();

            WriteLine("Applying Migrations...");

            context.Database.Migrate();
            if (!context.Products.Any())
            {
                WriteLine("Creating Seed Data...");

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Kayak",
                        Category = "Watersports",
                        Price = 275
                    },
                    new Product
                    {
                        Name = "Lifejacket",
                        Category = "Watersports",
                        Price = 48.95m
                    },
                    new Product
                    {
                        Name = "Soccer Ball",
                        Category = "Soccer",
                        Price = 19.50m
                    },
                    new Product
                    {
                        Name = "Thinking Cap",
                        Category = "Chess",
                        Price = 29.95m
                    });

                context.SaveChanges();
            }
            else
            {
                WriteLine("Seed Data Not Required...");
            }
        }
    }
}