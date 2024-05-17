using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Utilities;

public class FileUploader : IDisposable
{
    #region constructor
    public FileUploader(string path, string fileName, string[] accept, IFormFile file)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(file);

        FullPath = path;
        FileName = fileName;
        FileToUpload = file;
        GenerateFileName = false;
        Accept = accept;
    }

    public FileUploader(string path, string fileName, IFormFile file)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(file);

        FullPath = path;
        FileName = fileName;
        FileToUpload = file;
        GenerateFileName = false;
    }

    public FileUploader(string path, string[] accept, IFormFile file)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(file);

        FullPath = path;
        FileToUpload = file;
        FileName = null;
        GenerateFileName = true;
        Accept = accept;
    }

    public FileUploader(string path, IFormFile file)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(file);

        FullPath = path;
        FileToUpload = file;
        FileName = null;
        GenerateFileName = true;
    }
    #endregion

    #region properties
    public IFormFile FileToUpload { get; private set; }
    public string? FileName { get; private set; }
    public string FullPath { get; private set; }
    public string[] Accept { get; private set; } = Array.Empty<string>();
    public bool GenerateFileName { get; private set; }
    #endregion

    #region publics
    public async Task<(bool Status, string FileName, string Message)> UploadImageAsync(CancellationToken cancellationToken)
    {
        try
        {
            string fileExtension = Path.GetExtension(FileToUpload.FileName);

            if (!HasValidExtension(fileExtension))
                return (false, string.Empty, "invalid extension");

            string fileName = GenerateFileName ? GenerateNewName(fileExtension) : FileName ?? throw new ArgumentException("file name is invalid");

            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            using var stream = FileToUpload.OpenReadStream();
            using MemoryStream memoryStream = new();

            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            bool isValidFile = await ValidateImageFile(memoryStream, cancellationToken);

            if (isValidFile)
            {
                memoryStream.Position = 0;
                ImageOptimizer optimizer = new ImageOptimizer();
                optimizer.LosslessCompress(memoryStream);
            }

            if (!isValidFile)
                return (false, string.Empty, "invalid file - The bytes of this file have been manipulated and uploading it can be associated with security risks");

            using var fileStream = new FileStream($"{FullPath}/{fileName}", FileMode.Create);
            await memoryStream.CopyToAsync(fileStream, cancellationToken);

            memoryStream.Close();
            await memoryStream.DisposeAsync();

            stream.Close();
            await stream.DisposeAsync();

            fileStream.Close();
            await fileStream.DisposeAsync();

            return (true, $"{FullPath}/{fileName}", string.Empty);
        }
        catch (Exception exp)
        {
            return (false, string.Empty, exp.Message);
        }
    }

    public async Task<(bool Status, string FileRoute, string Message)> UploadImageAsync(string url,string folderName, CancellationToken cancellationToken)
    {
        string extension = Path.GetExtension(FileToUpload.FileName);
        string fileName = GenerateNewName(extension);

        if (!Directory.Exists(FullPath))
            Directory.CreateDirectory(FullPath);

        string route = Path.Combine(FullPath, fileName);
        using var stream = new MemoryStream();

        await FileToUpload.CopyToAsync(stream, cancellationToken);
        var content = stream.ToArray();
        await File.WriteAllBytesAsync(route, content, cancellationToken);

        var routeForDb = Path.Combine($"{url}{folderName}", fileName);
        return (true, routeForDb.Replace("\\","/"),"");
    }
    #endregion


    #region utilities
    private bool HasValidExtension(string fileExtension)
    {
        if (Accept.Length == 0)
            return true;

        if (!Accept.Any(a => a.Contains(fileExtension)))
            return false;

        return true;
    }
    private async Task<bool> ValidateImageFile(MemoryStream file, CancellationToken cancellationToken)
    {
        bool res = false;

        try
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream, cancellationToken);
            var imageBytes = memoryStream.ToArray();

            // only supported on window 6.1 or later
            using var image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
            return true;
        }
        catch (Exception exp)
        {
            res = false;
        }

        return res;
    }
    private string GenerateNewName(string fileExtension) => $"{Guid.NewGuid()}{fileExtension}";
    #endregion

    #region dispose pattern
    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
