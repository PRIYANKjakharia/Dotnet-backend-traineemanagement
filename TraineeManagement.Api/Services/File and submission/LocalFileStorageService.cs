namespace TraineeManagement.API.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;
    public LocalFileStorageService(IConfiguration configuration)
    {
        _rootPath = configuration["FileStorage:UploadPath"]!;
        if(!Directory.Exists(_rootPath)){
            Directory.CreateDirectory(_rootPath);
        }
    }
    public async Task DeleteAsync(string StoredFileName)
    {
        string path = Path.Combine(_rootPath , StoredFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        await Task.CompletedTask;
        // throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(string StoredFileName)
    {
        string path = Path.Combine(_rootPath, StoredFileName);

        return await Task.FromResult(File.Exists(path));
        // throw new NotImplementedException();
    }

    public async Task<Stream> OpenReadAsync(string StoredFileName)
    {
        string path = Path.Combine(_rootPath , StoredFileName);
        Stream stream = new FileStream(path, FileMode.Open , FileAccess.Read);

        return await Task.FromResult(stream);
        // throw new NotImplementedException();
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        string extension = Path.GetExtension(file.FileName);
        string StoredFileName = $"{Guid.NewGuid()}{extension}";
        string fullPath = Path.Combine(_rootPath , StoredFileName);

        using var stream = new FileStream(fullPath , FileMode.Create);

        await file.CopyToAsync(stream);
        return StoredFileName;
        // throw new NotImplementedException();
    }
}