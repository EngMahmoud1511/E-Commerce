using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string?> UploadPhotoAsync(IFormFile file);
        Task DeletePhotoAsync(string publicId);
    }
}
