﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public CarController (ICarService carService)
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

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _carService.Delete(id);
            return RedirectToAction("List");
        }
    }
}
