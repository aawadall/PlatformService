using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext? appDbContext)
        {
            if(!appDbContext?.Platforms.Any() ?? false)
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
                        Id = 1,
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform{
                        Id = 2,
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform{
                        Id = 3,
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free"
                    }
            };
        }
    }
}