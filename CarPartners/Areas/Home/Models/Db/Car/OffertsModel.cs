using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Areas.Home.Models.Db.Car
{
    public class OffertsModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime OfferDate { get; set; }
        public int CarId { get; set; }
        public CarModel Car { get; set; }
    }
}
