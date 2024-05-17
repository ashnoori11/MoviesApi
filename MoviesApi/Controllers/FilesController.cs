using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileProvider _fileProvider;
    public FilesController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFile(string fileName,CancellationToken cancellationToken)
    {
        var filePath = Path.Combine("Files", fileName);
        var fileInfo = _fileProvider.GetFileInfo(filePath);

        cancellationToken.ThrowIfCancellationRequested();

        if (fileInfo.Exists)
        {
            using var fileStream = fileInfo.CreateReadStream();
            return File(fileStream, "application/octet-stream");
        }

        return NotFound();
    }
}
