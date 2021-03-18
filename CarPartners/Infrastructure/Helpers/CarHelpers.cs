using NetCars.Areas.Home.Models.Db.Car;
using NetCars.Areas.Home.Models.View.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Infrastructure.Helpers
{
    public static class CarHelpers
    {
        public static CarModel ConvertToModel(CarView result)
        {
            var carModel = new CarModel
            {
                Name = result.Name,
                HorsePower = result.HorsePower,
                Color = result.Color,
                NumberOfDoors = result.NumberOfDoors,
                Seats = result.Seats,
                Mark = result.Mark,
                Capacity = result.Capacity,
                Cost = result.Cost,
                Mileage = result.Mileage

            };

            return carModel;
        }

        public static CarView ConvertToView(CarModel result)
        {
            var carView = new CarView
            {
                Id = result.Id,
                Name = result.Name,
                HorsePower = result.HorsePower,
                Color = result.Color,
                NumberOfDoors = result.NumberOfDoors,
                Seats = result.Seats,
                Mark = result.Mark,
                Capacity = result.Capacity,
                Cost = result.Cost,
                Mileage = result.Mileage
            };

            return carView;
        }

        public static CarModel MergeViewWithModel(CarModel model, CarView view)
        {
            model.Name = view.Name;
            model.HorsePower = view.HorsePower;
            model.Color = view.Color;
            model.NumberOfDoors = view.NumberOfDoors;
            model.Seats = view.Seats;
            model.Mark = view.Mark;
            model.Capacity = view.Capacity;
            model.Cost = view.Cost;
            model.Mileage = view.Mileage;

            return model;
        }

    }
}
