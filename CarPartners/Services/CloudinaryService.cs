using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetCars.Areas.Home.Models.Db.Car;
using NetCars.Context;
using NetCars.Infrastructure.Settings;
using NetCars.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Services
{
    public class CloudinaryService : ICloudinaryService
    {

        private readonly Cloudinary _cloudinary;
        private readonly NetCarsContext _context;

        public CloudinaryService(IOptions<CloudinarySettings> config, NetCarsContext context)
        {
            if (config.Value.CloudName != null && config.Value.ApiKey != null && config.Value.ApiSecret != null)
            {
                var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
                _cloudinary = new Cloudinary(account);
            }

            _context = context;
        }

        private double ConvertBytesToMegabytes(double bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private ImageUploadResult UploadToCloudinary(IFormFile file)
        {
            if (_cloudinary != null)
            {
                var uploadResult = new ImageUploadResult();

                if (file != null && file.Length > 0)
                {
                    using (var streamFile = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, streamFile)
                        };

                        if (file.ContentType.Contains("image"))
                        {
                            uploadParams.Transformation = new Transformation().Width(1000);
                        }

                        uploadResult = _cloudinary.Upload(uploadParams);
                    }

                    if (uploadResult.Error == null)
                    {
                        return uploadResult;
                    }
                }
            }

            return null;
        }

        public async Task<CarImageModel> SaveCarImage(ImageUploadResult uploadResult, string fileNameLong, int CarId)
        {
            var fileName = Path.GetFileNameWithoutExtension(fileNameLong);

            var image = new CarImageModel
            {
                Id = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                Name = fileName,
                Description = fileName,
                CarId = CarId
            };

            await _context.CarImages.AddAsync(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<bool> AddCarImage(IFormFile file, int carId)
        {
            var status = true;
            var uploadResult = UploadToCloudinary(file);

            if(uploadResult != null)
            {
                await SaveCarImage(uploadResult, file.FileName, carId);
            }

            return status;
        }

        public bool DeleteCarImage(string publicId)
        {
            if(_cloudinary != null)
            {
                var deleteParams = new DeletionParams(publicId);
                var result = _cloudinary.Destroy(deleteParams);

                if(result.Result == "ok")
                {
                    var toRemove = _context.CarImages.Find(publicId);
                    _context.CarImages.Remove(toRemove);

                    return _context.SaveChanges() > 0;
                }
            }

            return false;
        }
    }
}
