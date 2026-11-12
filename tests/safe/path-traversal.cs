using System.IO;
using Microsoft.AspNetCore.Http;

public class FileUploader
{
    public void Save(IFormFile file, string uploadDir)
    {
        var safeName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(uploadDir, safeName);
        var fullPath = Path.GetFullPath(filePath);
        if (!fullPath.StartsWith(Path.GetFullPath(uploadDir)))
            throw new InvalidOperationException("Path traversal detected");
        using var stream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(stream);
    }
}
