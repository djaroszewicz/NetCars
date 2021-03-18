using NetCars.Areas.Home.Models.Db.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Context
{
    public class Seed
    {
        public static async Task SeedData(NetCarsContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {

                var users = new List<User>
                {
                    new User
                    {
                        Id = "a",
                        UserName = "admin",
                        Email = "admin@admin.pl",
                    },
                    new User
                    {
                        Id = "u",
                        UserName = "user",
                        Email = "user@user.pl",
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "haslo");
                }
            }
        }
    }
}
