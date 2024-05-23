using Application.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetImages(string fileName,CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Images\\Actors", fileName);
        cancellationToken.ThrowIfCancellationRequested();

        if (System.IO.File.Exists(filePath))
        {
            return File(System.IO.File.ReadAllBytes(filePath), fileName.GetMimeType());
        }

        return NotFound();
    }

    [HttpGet("{fileDirectory}/{fileName}")]
    public async Task<IActionResult> GetFile(string fileDirectory,string fileName, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileDirectory, fileName);
        cancellationToken.ThrowIfCancellationRequested();

        if (System.IO.File.Exists(fullPath))
        {
            return File(System.IO.File.ReadAllBytes(fullPath), fileName.GetMimeType());
        }

        return NotFound();
    }
}
