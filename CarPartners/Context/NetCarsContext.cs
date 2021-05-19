using NetCars.Areas.Home.Models.Db.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCars.Areas.Home.Models.Db.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Context
{
    public class NetCarsContext : IdentityDbContext<User>
    {
        public NetCarsContext(DbContextOptions<NetCarsContext> options) : base(options) { }

        //Tutaj dodajemy DbSety

        public DbSet<CarModel> Cars { get; set; }
        public DbSet<OffertsModel> Offerts { get; set; }
        public DbSet<CarImageModel> CarImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
