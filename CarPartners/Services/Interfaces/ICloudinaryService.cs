using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Services.Interfaces
{
    public interface ICloudinaryService
    {

        Task<bool> AddCarImage(IFormFile files, int carId);
        bool DeleteCarImage(string publicId);

    }
}
