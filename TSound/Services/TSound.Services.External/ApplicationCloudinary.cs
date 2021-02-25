using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace TSound.Services.External
{
    public class ApplicationCloudinary
    {
        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile file, string fileName)
        {
            if (file == null)
                return null;

            byte[] image;

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            image = memoryStream.ToArray();

            using var destinationStream = new MemoryStream(image);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, destinationStream),
                PublicId = fileName,
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return result.Url.AbsoluteUri;
        }

        public static void DeleteImage(Cloudinary cloudinary, string name, string folderName)
        {
            /*var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { $"{folderName}/{name}" },
                Invalidate = true,
                ResourceType = ResourceType.Raw,
            };

            cloudinary.DeleteResources(delParams);*/

            var delParams = new DeletionParams($"{folderName}/{name}");

            cloudinary.Destroy(delParams);
        }
    }
}
