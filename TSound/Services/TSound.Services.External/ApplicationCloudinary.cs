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
        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile image, string fileName)
        {
            if (image != null)
            {
                byte[] destinationImage;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    destinationImage = memoryStream.ToArray();
                }

                using (var ms = new MemoryStream(destinationImage))
                {
                    // Cloudinary doesn't work with [?, &, #, \, %, <, >]
                    fileName = fileName.Replace("&", "And");
                    fileName = fileName.Replace("#", "sharp");
                    fileName = fileName.Replace("?", "questionMark");
                    fileName = fileName.Replace("\\", "right");
                    fileName = fileName.Replace("%", "percent");
                    fileName = fileName.Replace(">", "greater");
                    fileName = fileName.Replace("<", "lower");

                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(fileName, ms),
                        PublicId = fileName,
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    return uploadResult.SecureUri.AbsoluteUri;
                }
            }

            return null;
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
