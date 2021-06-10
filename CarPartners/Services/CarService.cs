using NetCars.Context;
using Microsoft.EntityFrameworkCore;
using NetCars.Areas.Home.Models.Db.Car;
using NetCars.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NetCars.Areas.Home.Models.Db.Account;

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
            return await _context.Cars
                .Include(i => i.CarImage)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<CarModel>> GetAll()
        {
            return await _context.Cars
                .Include(i => i.CarImage)
                .ToListAsync();
        }

        public async Task<List<CarModel>> GetAllByName()
        {
            return await _context.Cars
                .Include(i => i.CarImage)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<List<CarModel>> GetAllByCost()
        {
            return await _context.Cars
                .Include(i => i.CarImage)
                .OrderBy(i => i.Cost)
                .ToListAsync();
        }

        public async Task<bool> Update(CarModel car)
        {
            _context.Cars.Update(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<OffertsModel>> OffertsList()
        {
            return await _context.Offerts
                .Include(c => c.Car).ToListAsync();
        }

        public async Task<bool> UpdateOffer(List<OffertsModel> offerts)
        {
            foreach(var offer in offerts)
            {
                _context.Offerts.Update(offer);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddCarToFavorite(CarModel car, string userId)
        {
            var favoriteModel = new CarFavoritedModel
            {
                UserId = userId,
                Car = car
            };

            _context.CarFavorites.Add(favoriteModel);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CarModel> GetFavoriteCar()
        {
            var favoriteCarId = await _context.CarFavorites.SingleOrDefaultAsync();

            return await _context.Cars
                .Include(i => i.CarImage)
                .SingleOrDefaultAsync(a => a.Id == favoriteCarId.CarId);
        }
    }
}
