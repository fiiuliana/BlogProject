using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public interface IPhotoService
    {
      public Task<ImageUploadResult> AddPhotoAsync(IFormFile file);  // from Cloudinary
      public Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
