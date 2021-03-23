using NetCars.Context;
using NetCars.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Services
{
    public class HomeService : IHomeService
    {
        private readonly NetCarsContext _context;

        public HomeService(NetCarsContext context)
        {
            _context = context;
        }
    }
}
