using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TSound.Services.External
{
    public interface IApplicationCloudinary
    {
        Task<string> UploadImageAsync(IFormFile file, string fileName);
    }
}
