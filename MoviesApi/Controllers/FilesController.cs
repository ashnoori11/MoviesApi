using Application.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.FileStorageServices.Liara;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
public class FilesController : ControllerBase
{
    private readonly ILiaraStorageService _s3Service;
    public FilesController(ILiaraStorageService s3Service)
    {
        _s3Service = s3Service;
    }


    [HttpGet("{fileName}",Name = "GetImages")]
    public async Task<IActionResult> Get(string fileName,CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Images\\Actors", fileName);
        cancellationToken.ThrowIfCancellationRequested();

        if (System.IO.File.Exists(filePath))
        {
            return File(System.IO.File.ReadAllBytes(filePath), fileName.GetMimeType());
        }

        return NotFound();
    }

    [HttpGet("{fileDirectory}/{fileName}",Name = "GetFile")]
    public async Task<IActionResult> Get(string fileDirectory,string fileName, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileDirectory, fileName);
        cancellationToken.ThrowIfCancellationRequested();

        if (System.IO.File.Exists(fullPath))
        {
            return File(System.IO.File.ReadAllBytes(fullPath), fileName.GetMimeType());
        }

        return NotFound();
    }

    [HttpGet(Name = "GetAllAvailableBuckets")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var res = await _s3Service.GetAllBucketsAsync(cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet("{expiresInHours:int}",Name = "GetAllUrlsFromS3FileStorage")]
    public async Task<IActionResult> Get(int expiresInHours, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _s3Service.GetAllUrlsFromS3FileStorageAsync(cancellationToken, expiresInHours);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost(("{file}"),Name ="UploadFile")]
    public async Task<IActionResult> Post(IFormFile file,CancellationToken cancellationToken)
    {
        try
        {
            var res = await _s3Service.UploadToS3Async(file, cancellationToken);

            return Ok();
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost("{fileName}",Name ="DownloadFromS3")]
    public async Task<IActionResult> Post(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            string directoryName = $"{Directory.GetCurrentDirectory()}\\Files\\Images\\LiaraStorage\\";
            var res = await _s3Service.DownloadFromS3Async(fileName, directoryName,cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpDelete("{objectName}",Name ="DeleteObjectByKey")]
    public async Task<IActionResult> Delete(string objectName,CancellationToken cancellationToken)
    {
        try
        {
            var res = await _s3Service.DeleteFromS3StorageAsync(objectName,cancellationToken);
            return Ok(res.Message);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }
}
