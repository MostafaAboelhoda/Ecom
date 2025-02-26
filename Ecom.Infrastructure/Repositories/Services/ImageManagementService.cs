using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _FileProvider;
        public ImageManagementService(IFileProvider FileProvider)
        {
            _FileProvider = FileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc = new List<string>();
            var imageDirectory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(imageDirectory) is not true)
            {
                Directory.CreateDirectory(imageDirectory);
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // get image Name 
                    var imageName = file.FileName;

                    var imageSrc = $"Images/{src}/{imageName}";

                    var root = Path.Combine(imageDirectory, imageName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    saveImageSrc.Add(imageSrc);
                }
            }
            return saveImageSrc;
        }

        public async Task DeleteImageAsycn(string src)
        {
            var info = _FileProvider.GetFileInfo(src);

            var root = info.PhysicalPath;

            File.Delete(root);
        }
    }
}
