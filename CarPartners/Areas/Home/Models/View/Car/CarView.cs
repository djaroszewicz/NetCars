using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Areas.Home.Models.View.Car
{
    public class CarView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HorsePower { get; set; }
        public string Color { get; set; }
        public int NumberOfDoors { get; set; }
        public int Seats { get; set; }
        public string Mark { get; set; }
        public double Capacity { get; set; }
        public double Cost { get; set; }
        public double Mileage { get; set; }
        public IFormFile CarFileImg { get; set; }
        public string CarImageUrl { get; set; }
    }
}
