using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCars.Areas.Home.Models.Db.Account;
using NetCars.Areas.Home.Models.Db.Car;
using NetCars.Areas.Home.Models.View.Car;
using NetCars.Infrastructure.Helpers;
using NetCars.Services.Interfaces;

namespace NetCars.Areas.Home.Controllers
{
    [Area("home")]
    [Route("home/{controller}/{action=List}/{id?}")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UserManager<User> _userManager;

        public CarController(ICarService carService, ICloudinaryService cloudinaryService, UserManager<User> userManager)
        {
            _carService = carService;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(int optionValue)
        {
            var cars = await _carService.GetAll();


            switch (optionValue)
            {
                case 1:
                    var orderByNameCars = await _carService.GetAllByName();
                    return View(orderByNameCars);
                case 2:
                    var orderByCostCars = await _carService.GetAllByCost();
                    return View(orderByCostCars);
                default:
                    return View(cars);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var model = await _carService.Get(Id);
            return View(CarHelpers.ConvertToView(model));
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Offerts()
        {
            var offerts = await _carService.OffertsList();
            var carList = await _carService.GetAll();

            if(offerts.Count == 0)
            {
                Random randomNumber = new Random();
                int randomCar = randomNumber.Next(carList.Count);
                var carOffer = await _carService.Get(randomCar);

                var newOffer = new OffertsModel
                {
                    Car = carOffer,
                    OfferDate = DateTime.Now
                };

                List<OffertsModel> offerCarList = new List<OffertsModel>();
                offerCarList.Add(newOffer);

                await _carService.UpdateOffer(offerCarList);
            }


            foreach(var newOffer in offerts)
            {
                if((newOffer.OfferDate.AddSeconds(5)) < DateTime.Now)
                {
                    Random randomNumber = new Random();
                    int randomCar = randomNumber.Next(carList.Count);
                    var carOffer = await _carService.Get(randomCar);

                    newOffer.OfferDate = DateTime.Now;
                    newOffer.Car = carOffer;
                }
            }

            await _carService.UpdateOffer(offerts);

            var offerCar = await _carService.GetFavoriteCar();

            return View(offerCar);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _carService.Get(id);

            return View(CarHelpers.ConvertToView(model));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarView result)
        {
            if(!ModelState.IsValid)
            {
                return View(result);
            }

            var resultModel = CarHelpers.ConvertToModel(result);
            await _carService.Create(resultModel);
            await _cloudinaryService.AddCarImage(result.CarFileImg, resultModel.Id);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> AddCarToFavorite(CarModel car)
        {
            var user = await _userManager.GetUserAsync(User);
            await _carService.AddCarToFavorite(car, user.Id);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarView result)
        {
            if(!ModelState.IsValid)
            {
                return View(result);
            }
            
            var model = await _carService.Get(result.Id);
            var updateModel = CarHelpers.MergeViewWithModel(model, result);

            await _carService.Update(updateModel);

            if(result.CarFileImg != null)
            {
                if (model.CarImage != null)
                {
                    _cloudinaryService.DeleteCarImage(model.CarImage.Id);
                }

                await _cloudinaryService.AddCarImage(result.CarFileImg, model.Id);
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _carService.Delete(id);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Details()
        {
            var carModel = await _carService.Get(6);

            return View(CarHelpers.ConvertToView(carModel));
        }
    }
}