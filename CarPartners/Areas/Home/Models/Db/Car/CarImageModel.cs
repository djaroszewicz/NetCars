using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Areas.Home.Models.Db.Car
{
    public class CarImageModel
    {
        [Key]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CarId { get; set; }
        public CarModel Car { get; set; }
    }
}
