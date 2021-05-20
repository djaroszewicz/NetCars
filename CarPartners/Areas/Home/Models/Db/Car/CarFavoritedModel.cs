using NetCars.Areas.Home.Models.Db.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Areas.Home.Models.Db.Car
{
    public class CarFavoritedModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CarId { get; set; }
        public CarModel Car { get; set; }
    }
}
