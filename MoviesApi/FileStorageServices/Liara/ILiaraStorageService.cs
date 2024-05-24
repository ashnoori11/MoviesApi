namespace MoviesApi.FileStorageServices.Liara;

public interface ILiaraStorageService
{
    Task<(bool Status, string FileName, string Message)> UploadToS3Async(IFormFile image, CancellationToken cancellationToken);
    Task<(bool Status, string FilePath, string Message)> DownloadFromS3Async(string objectkey, string directory, CancellationToken cancellationToken);
    Task<(bool Status, string Message)> DeleteFromS3StorageAsync(string objectkey, CancellationToken cancellationToken);
    Task<string[]> GetAllBucketsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetAllUrlsFromS3FileStorageAsync(CancellationToken cancellationToken, int expiresInHours = 0);
}