using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCars.Infrastructure.Helpers;
using NetCars.Services.Interfaces;

namespace CarPartners.Areas.Home.Controllers
{
    [Area("home")]
    [Route("home/{controller}/{action=List}/{id?}")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var cars = await _carService.GetAll();
            return View(cars);
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
    }
}