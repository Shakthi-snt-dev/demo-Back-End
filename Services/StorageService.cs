using Microsoft.AspNetCore.Hosting;

namespace FlowTap.Api.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public StorageService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var provider = _configuration["Storage:Provider"] ?? "Local";
        
        if (provider == "Local")
        {
            var uploadPath = Path.Combine(_environment.WebRootPath ?? "wwwroot", 
                _configuration["Storage:LocalPath"] ?? "uploads");
            Directory.CreateDirectory(uploadPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var file = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            return $"/uploads/{uniqueFileName}";
        }
        else if (provider == "S3")
        {
            // AWS S3 implementation would go here
            throw new NotImplementedException("S3 storage not yet implemented");
        }

        throw new Exception("Invalid storage provider");
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return Task.FromResult(false);

        var provider = _configuration["Storage:Provider"] ?? "Local";
        
        if (provider == "Local")
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_environment.WebRootPath ?? "wwwroot",
                _configuration["Storage:LocalPath"] ?? "uploads", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}

