using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Birth.Application.FileService
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid");


            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{fileName}";
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName = "uploads")
        {
            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("File stream is invalid");

            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return $"/{folderName}/{fileName}";
        }

        public Task<bool> DeleteFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return Task.FromResult(false);

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}



//public Task DeleteFile(string fileUrl)
//{
//    if (string.IsNullOrEmpty(fileUrl))
//        return Task.CompletedTask;

//    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileUrl.TrimStart('/'));

//    if (File.Exists(filePath))
//    {
//        File.Delete(filePath);
//    }

//    return Task.CompletedTask;
//}