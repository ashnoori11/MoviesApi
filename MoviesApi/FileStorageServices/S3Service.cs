using Amazon.S3.Model;
using Amazon.S3;
using DotNetEnv;
using Amazon.Runtime;
using System.Text;

namespace MoviesApi.FileStorageServices;

public class S3Service : IS3Service
{
    private BasicAWSCredentials Credentials { get; set; }

    public S3Service()
    {
        Env.Load();
        Credentials = new Amazon.Runtime.BasicAWSCredentials(Env.GetString("LIARA_ACCESS_KEY"), Env.GetString("LIARA_SECRET_KEY"));
    }

    private string _bucketName;
    public string BucketName
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_bucketName))
                _bucketName = Env.GetString("LIARA_BUCKET_NAME");

            return _bucketName;
        }
    }


    private string _endpoint;
    public string Endpoint
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
                _endpoint = Env.GetString("LIARA_ENDPOINT");

            return _endpoint;
        }
    }

    private AmazonS3Config _configurations;
    public AmazonS3Config Configurations
    {
        get
        {
            if (_configurations is null)
                _configurations = new AmazonS3Config
                {
                    ServiceURL = this.Endpoint,
                    ForcePathStyle = true,
                    SignatureVersion = "4"
                };

            return _configurations;
        }
    }

    public async Task<(bool Status, string FileName, string Message)> UploadToS3Async(IFormFile image,CancellationToken cancellationToken)
    {
        using var client = new AmazonS3Client(this.Credentials, this.Configurations);
        string objectKey = Guid.NewGuid().ToString() + image.FileName;

        try
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = this.BucketName,
                Key = objectKey,
                InputStream = memoryStream,
            };

            await client.PutObjectAsync(request, cancellationToken);
            string fileUrl = $"{this.Endpoint}/{this.BucketName}/{objectKey}";
            return (true, fileUrl, $"File '{objectKey}' uploaded successfully.");
        }

        catch (AmazonS3Exception e)
        {
            return (false, string.Empty, $"Upload file '{image.FileName}' has been faield. error message : {e.Message}");
        }
    }

    public async Task<(bool Status, string FilePath, string Message)> DownloadFromS3Async(string objectkey,string directory,CancellationToken cancellationToken)
    {
        try
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using var client = new AmazonS3Client(this.Credentials, this.Configurations);
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = this.BucketName,
                Key = objectkey
            };

            using GetObjectResponse response = await client.GetObjectAsync(request,cancellationToken);
            using Stream responseStream = response.ResponseStream;

            string downloadPath = $"{directory}{objectkey}";

            using FileStream fileStream = File.Create(downloadPath);
            await responseStream.CopyToAsync(fileStream,cancellationToken);

            return (true, downloadPath, $"File '{objectkey}' downloaded successfully.");
        }
        catch (AmazonS3Exception e)
        {
            return (false,objectkey, $"Error: {e.Message}");
        }
    }

    public async Task<(bool Status,string Message)> DeleteFromS3StorageAsync(string objectkey,CancellationToken cancellationToken)
    {
        try
        {
            using var client = new AmazonS3Client(this.Credentials, this.Configurations);
            DeleteObjectRequest deleteRequest = new DeleteObjectRequest
            {
                BucketName = this.BucketName,
                Key = objectkey
            };

            await client.DeleteObjectAsync(deleteRequest,cancellationToken); 
            return (true, $"File '{objectkey}' deleted successfully.");
        }
        catch (AmazonS3Exception e)
        {
            return (false,$"Error : {e.Message}");
        }
    }

    public async Task<string[]> GetAllBucketsAsync(CancellationToken cancellationToken)
    {
        using var client = new AmazonS3Client(this.Credentials, this.Configurations);

        ListBucketsResponse response = await client.ListBucketsAsync(cancellationToken);
        return response.Buckets.Select(bucket => bucket.BucketName).ToArray();
    }

    public async Task<List<string>> GetAllUrlsFromS3FileStorageAsync(CancellationToken cancellationToken, int expiresInHours = 0)
    {
        using var client = new AmazonS3Client(this.Credentials, this.Configurations);
        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = this.BucketName
        };

        ListObjectsV2Response response = await client.ListObjectsV2Async(request);

        List<string> urls = new();
        StringBuilder sb = new();
        foreach (S3Object entry in response.S3Objects)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = this.BucketName,
                Key = entry.Key,
                Expires = DateTime.Now.AddHours(expiresInHours)
            };

            sb.Append(client.GetPreSignedURL(urlRequest));
            urls.Add($"File: {entry.Key}, URL: {sb.ToString()}");
            sb.Clear();
        }

        return urls;
    }
}
