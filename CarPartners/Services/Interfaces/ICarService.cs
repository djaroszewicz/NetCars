using NetCars.Areas.Home.Models.Db.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Services.Interfaces
{
    public interface ICarService
    {
        Task<bool> Create(CarModel car);
        Task<CarModel> Get(int id);
        Task<List<CarModel>> GetAll();
        Task<List<CarModel>> GetAllByName();
        Task<List<CarModel>> GetAllByCost();
        Task<bool> Update(CarModel car);
        Task<bool> Delete(int id);
        Task<bool> AddCarToFavorite(CarModel car, string userId);
        Task<List<OffertsModel>> OffertsList();
        Task<bool> UpdateOffer(List<OffertsModel> offerts);

        Task<CarModel> GetFavoriteCar();
    }
}
