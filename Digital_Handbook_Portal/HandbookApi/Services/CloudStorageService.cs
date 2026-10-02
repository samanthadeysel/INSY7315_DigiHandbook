using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace HandbookApi.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public CloudStorageService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _bucketName = configuration["GoogleCloudStorage:BucketName"] 
                ?? throw new InvalidOperationException("Bucket name not configured.");
            
            var relativePath = configuration["GoogleCloudStorage:CredentialFilePath"] 
                ?? "Credentials/digitalhandbook-37a30108df42.json";
            
            var credentialPath = Path.Combine(environment.ContentRootPath, relativePath);

            var credential = GoogleCredential.FromFile(credentialPath);
            _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file stream.");

            var fileExtension = Path.GetExtension(file.FileName);
            var objectName = $"{folderName}/{Guid.NewGuid()}{fileExtension}";

            using (var stream = file.OpenReadStream())
            {
                var uploadedObject = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: objectName,
                    contentType: file.ContentType,
                    source: stream
                );

                return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return;

            var uri = new Uri(fileUrl);
            var objectName = uri.AbsolutePath.TrimStart('/').Replace($"{_bucketName}/", "");

            await _storageClient.DeleteObjectAsync(_bucketName, objectName);
        }
    }
}