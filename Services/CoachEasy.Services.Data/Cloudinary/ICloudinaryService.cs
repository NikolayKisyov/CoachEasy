namespace CoachEasy.Services.Data.Cloudinary
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadPhotoAsync(IFormFile picture, string name, string folderName);
    }
}
