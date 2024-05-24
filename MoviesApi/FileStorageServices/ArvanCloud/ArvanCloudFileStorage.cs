//using Amazon.S3.Model;
//using Amazon.S3;
//using Amazon.S3.Util;
//using System.Reflection;
//using System.Xml;
//using Amazon.Runtime;
//using DotNetEnv;

//namespace MoviesApi.FileStorageServices.ArvanCloud;

//public class ArvanCloudFileStorage : IArvanCloudFileStorage
//{
//    private static IAmazonS3 _s3Client;
//    public ArvanCloudFileStorage()
//    {
//        Env.Load();
//        var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Env.GetString("ArvanCloud_ACCESS_KEY"), Env.GetString("ArvanCloud_SECRET_KEY"));
//        var config = new AmazonS3Config { ServiceURL = Env.GetString("ArvanCloud_ENDPOINT") };

//        _s3Client = new AmazonS3Client(awsCredentials, config);
//    }

//    public async Task CreatingBucketAsync(IAmazonS3 client, string bucketName)
//    {
//        try
//        {
//            var putBucketRequest = new PutBucketRequest
//            {
//                BucketName = bucketName,
//                UseClientRegion = true
//            };

//            var putBucketResponse = await client.PutBucketAsync(putBucketRequest);

//        }
//        catch (AmazonS3Exception ex)
//        {
//            Console.WriteLine($"Error creating bucket: '{ex.Message}'");
//        }
//    }
//    public async Task HeadBucketAsync(IAmazonS3 client, string bucketName)
//    {
//        bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);

//        if (bucketExists)
//        {
//            Console.WriteLine("Bucket Exists");
//        }
//        else
//        {
//            Console.WriteLine("Bucket DOES NOT Exists");
//        }

//        Console.WriteLine(bucketExists);
//    }
//    public async Task<ListBucketsResponse> GetBuckets(IAmazonS3 client)
//    {
//        return await client.ListBucketsAsync();
//    }
//    public void DisplayBucketList(List<S3Bucket> bucketList)
//    {
//        bucketList
//            .ForEach(b => Console.WriteLine($"Bucket name: {b.BucketName}, created on: {b.CreationDate}"));
//    }
//    public async Task DeletingBucketAsync(IAmazonS3 client, string bucketName)
//    {
//        try
//        {
//            var deleteResponse = await client.DeleteBucketAsync(bucketName);
//            Console.WriteLine($"\nResult: {deleteResponse.HttpStatusCode.ToString()}");
//            Console.WriteLine("Bucket successfully deleted");
//        }
//        catch (AmazonS3Exception ex)
//        {
//            Console.WriteLine($"Error: {ex.Message}");
//        }
//    }


//    // -----------------------------

//    public async Task UploadObjectFromFileAsync(string bucketName,string objectName,string filePath,CancellationToken cancellationToken)
//    {
//        try
//        {
//            var putRequest = new PutObjectRequest
//            {
//                BucketName = bucketName,
//                Key = objectName,
//                FilePath = filePath,
//                ContentType = "text/plain"
//            };

//            putRequest.Metadata.Add("x-amz-meta-title", "someTitle");

//            PutObjectResponse response = await client.PutObjectAsync(putRequest);

//            foreach (PropertyInfo prop in response.GetType().GetProperties())
//            {
//                Console.WriteLine($"{prop.Name}: {prop.GetValue(response, null)}");
//            }

//            Console.WriteLine($"Object {OBJECT_NAME} added to {bucketName} bucket");
//        }
//        catch (AmazonS3Exception e)
//        {
//            Console.WriteLine($"Error: {e.Message}");
//        }
//    }
//    public async Task UploadObjectAsync(string bucketName,string keyName,string filePath, CancellationToken cancellationToken)
//    {
//        // Create list to store upload part responses.
//        List<UploadPartResponse> uploadResponses = new();

//        // Setup information required to initiate the multipart upload.
//        InitiateMultipartUploadRequest initiateRequest = new()
//        {
//            BucketName = bucketName,
//            Key = keyName,
//        };

//        // Initiate the upload.
//        InitiateMultipartUploadResponse initResponse =
//            await client.InitiateMultipartUploadAsync(initiateRequest);

//        // Upload parts.
//        long contentLength = new FileInfo(filePath).Length;
//        long partSize = 400 * (long)Math.Pow(2, 20); // 400 MB

//        try
//        {
//            Console.WriteLine("Uploading parts");

//            long filePosition = 0;
//            for (int i = 1; filePosition < contentLength; i++)
//            {
//                UploadPartRequest uploadRequest = new()
//                {
//                    BucketName = bucketName,
//                    Key = keyName,
//                    UploadId = initResponse.UploadId,
//                    PartNumber = i,
//                    PartSize = partSize,
//                    FilePosition = filePosition,
//                    FilePath = filePath,
//                };

//                // Track upload progress.
//                uploadRequest.StreamTransferProgress +=
//                    new EventHandler<StreamTransferProgressArgs>(UploadPartProgressEventCallback);

//                // Upload a part and add the response to our list.
//                uploadResponses.Add(await client.UploadPartAsync(uploadRequest));

//                filePosition += partSize;
//            }

//            // Setup to complete the upload.
//            CompleteMultipartUploadRequest completeRequest = new()
//            {
//                BucketName = bucketName,
//                Key = keyName,
//                UploadId = initResponse.UploadId,
//            };
//            completeRequest.AddPartETags(uploadResponses);

//            // Complete the upload.
//            CompleteMultipartUploadResponse completeUploadResponse =
//                await client.CompleteMultipartUploadAsync(completeRequest);

//            Console.WriteLine($"Object {keyName} added to {bucketName} bucket");
//        }
//        catch (Exception exception)
//        {
//            Console.WriteLine($"An AmazonS3Exception was thrown: {exception.Message}");

//            // Abort the upload.
//            AbortMultipartUploadRequest abortMPURequest = new()
//            {
//                BucketName = bucketName,
//                Key = keyName,
//                UploadId = initResponse.UploadId,
//            };
//            await client.AbortMultipartUploadAsync(abortMPURequest);
//        }
//    }
//    public void UploadPartProgressEventCallback(object sender, StreamTransferProgressArgs e, CancellationToken cancellationToken)
//    {
//        Console.WriteLine($"{e.TransferredBytes}/{e.TotalBytes}");
//    }
//    public async Task ReadObjectDataAsync(string bucketName, string keyName, CancellationToken cancellationToken)
//    {
//        string responseBody = string.Empty;

//        try
//        {
//            GetObjectRequest request = new GetObjectRequest
//            {
//                BucketName = bucketName,
//                Key = keyName,
//            };

//            using (GetObjectResponse response = await client.GetObjectAsync(request))
//            using (Stream responseStream = response.ResponseStream)
//            using (StreamReader reader = new StreamReader(responseStream))
//            {
//                // Assume you have "title" as medata added to the object.
//                string title = response.Metadata["x-amz-meta-title"];
//                string contentType = response.Headers["Content-Type"];

//                Console.WriteLine($"Object metadata, Title: {title}");
//                Console.WriteLine($"Content type: {contentType}");

//                // Retrieve the contents of the file.
//                responseBody = reader.ReadToEnd();

//                // Write the contents of the file to disk.
//                string filePath = keyName;

//                Console.WriteLine("File successfully downloaded");
//            }
//        }
//        catch (AmazonS3Exception e)
//        {
//            // If the bucket or the object do not exist
//            Console.WriteLine($"Error: '{e.Message}'");
//        }
//    }
//    public async Task DeleteObjectHelper(string objectKey, CancellationToken cancellationToken)
//    {
//        try
//        {
//            // Create the request
//            DeleteObjectsRequest request = new()
//            {
//                BucketName = objectKey,
//                Objects = new List<KeyVersion> { new KeyVersion() { Key = objectKey, VersionId = null } }
//            };

//            // Submit the request
//            DeleteObjectsResponse response = await _s3Client.DeleteObjectsAsync(request);

//            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
//            Console.WriteLine($"Object {OBJECT_NAME} successfully deleted from {BUCKET_NAME} bucket");
//        }
//        catch (AmazonS3Exception amazonS3Exception)
//        {
//            Console.WriteLine("An AmazonS3Exception was thrown. Exception: " + amazonS3Exception.ToString());
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine("Exception: " + e.ToString());
//        }
//    }
//}


