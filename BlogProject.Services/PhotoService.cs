using BlogProject.Models.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class PhotoService : IPhotoService
    {
        // use the upload and delete from
        public readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinaryOptions> config)
        {
            // use class to connect to the account
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            //wrapped around the Cloudinary services
            _cloudinary = new Cloudinary(account);   
            //within appsettings.json user must create the cloudinary options
           // modify configure services in start-up - Cloudinary - references to models 
        } 

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); // from cloudinary
            if (file.Length > 0)  // if the length of the passed in file = actual a file
            {
                using (var stream = file.OpenReadStream())  // returnifn the file into a stream
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),   //wants the actual file and the stream
                        Transformation = new Transformation().Height(300).Width(500).Crop("fill") // set the size
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams); 
                }
            }
            return uploadResult; 
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }
    }
}
