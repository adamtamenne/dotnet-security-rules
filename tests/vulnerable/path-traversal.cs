using System.IO;
using Microsoft.AspNetCore.Http;

public class FileUploader
{
    public void Save(IFormFile file, string uploadDir)
    {
        var filePath = Path.Combine(uploadDir, file.FileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(stream);
    }
}
