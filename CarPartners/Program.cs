using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPartners.Areas.Home.Models.Db.Account;
using CarPartners.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarPartners
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<NetCarsContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    context.Database.Migrate();
                    Seed.SeedData(context, userManager).Wait();

                    logger.LogInformation("Migracja zakonczona pomyœlnie");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Wystapil problem przy migracji danych.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
