using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext? appDbContext, bool isProd)
        {
            if (isProd)
            {
                System.Console.WriteLine("--> Applying Migrations...");

                try
                {
                    appDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Could not run migrations: {ex.Message}");
                    
                }
            }

            if (!appDbContext?.Platforms.Any() ?? false)
            {
                System.Console.WriteLine("--> Seeding Data...");
                var platforms = GenerateMockData();
                appDbContext.AddRange(
                    platforms
                );

                appDbContext.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("--> We already have data");
            }
        }

        private static IEnumerable<Platform> GenerateMockData()
        {
            return new List<Platform>{
                new Platform{
                        // Id = 1,
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform{
                        // Id = 2,
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform{
                        // Id = 3,
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free"
                    }
            };
        }
    }
}