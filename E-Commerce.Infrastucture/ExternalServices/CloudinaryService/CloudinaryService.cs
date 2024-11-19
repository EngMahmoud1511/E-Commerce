using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_Commerce.Application.Interfaces;
using E_Commerce.Infrastucture.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastucture.ExternalServices.CloudinaryService
{
    public class CloudinaryService:ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;


        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            _cloudinary = new Cloudinary(new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret));
        }

        

        public async Task<string?> UploadPhotoAsync(IFormFile file)
        {
            if (file.Length == 0)
                return null;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.ToString();
        }

        public async Task DeletePhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deletionParams);
        }
    }
}
