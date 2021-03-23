using NetCars.Context;
using Microsoft.EntityFrameworkCore;
using NetCars.Areas.Home.Models.Db.Car;
using NetCars.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Services
{
    public class CarService : ICarService
    {

        private readonly NetCarsContext _context;

        public CarService(NetCarsContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(CarModel car)
        {
            await _context.Cars.AddAsync(car);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var car = await _context.Cars.SingleOrDefaultAsync(a => a.Id == id);

            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CarModel> Get(int id)
        {
            return await _context.Cars.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<CarModel>> GetAll()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<bool> Update(CarModel car)
        {
            _context.Cars.Update(car);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
