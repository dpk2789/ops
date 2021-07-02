using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApp.UI.Helpers
{
    public class FileManager
    {
        private IWebHostEnvironment _env;
        private List<ProductImageFileManager> Files;
        private int Id;

        private readonly List<(int Width, int Height)> imgSizes = new List<(int, int)>
            {
                (480, 270),
                (640, 360),
                (1280, 720),
                (1920, 1080)
            };

        public FileManager(IWebHostEnvironment env)
        {
            _env = env;
            Files = new List<ProductImageFileManager>();
        }

        public ProductImageFileManager GetFile(Guid id) => Files.FirstOrDefault(x => x.Id == id);

        public ProductImageFileManager GetFile(Guid id, int width) => Files.FirstOrDefault(x => x.Id == id && x.Width == width);

        public IEnumerable<ProductImageFileManager> GetFiles() => Files;

        public IEnumerable<Guid> GetOptimizedFiles() =>
            Files
            .Where(x => x.Width > 0)
            .Select(x => x.Id)
            .Distinct();

        public void SaveFile(IFormFile file)
        {
            var name = RandomName();
            var save_path = Path.Combine(_env.WebRootPath, name);

            using (var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }

            Files.Add(new ProductImageFileManager
            {
                Id = Guid.NewGuid(),
                Width= (int)file.Length,
                RelativePath = $"/{name}",
                GlobalPath = save_path
            });
        }

        private string RandomName(string prefix = "") =>
            $"img{prefix}_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.png";     

       

        public FileStream GetImageStream(Guid id, int width)
        {
            var path = GetFile(id, GetBestWidth(width)).GlobalPath;

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        private int GetBestWidth(int width)
        {
            foreach (var (Width, Height) in imgSizes)
                if (Width >= width)
                    return Width;

            return imgSizes[imgSizes.Count - 1].Width;
        }
    }

    public class ProductImageFileManager
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int? Width { get; set; }
        public string RelativePath { get; set; }
        public string GlobalPath { get; set; }
        public string Type { get; set; }
        public string Extention { get; set; }
    }
}
