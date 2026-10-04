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
                ?? configuration["GCS:BucketName"]
                ?? throw new InvalidOperationException("Bucket name not configured.");

            string secretMountPath = configuration["GoogleCloudStorage:SecretMountPath"] ?? "/app/Credentials/digitalhandbook-37a30108df42.json";

            string relativePath = configuration["GoogleCloudStorage:CredentialFilePath"]
                ?? "Credentials/digitalhandbook-37a30108df42.json";

            string localCredentialPath = Path.Combine(environment.ContentRootPath, relativePath);

            string activePath = File.Exists(secretMountPath)
                ? secretMountPath
                : (File.Exists(localCredentialPath) ? localCredentialPath : string.Empty);

            if (!string.IsNullOrEmpty(activePath))
            {
                var credential = GoogleCredential.FromFile(activePath);
                _storageClient = StorageClient.Create(credential);
            }
            else
            {
                var jsonCredentials = configuration["GoogleCloudStorage:JsonCredentials"]
                    ?? configuration["digitalhandbook-37a30108df42"];

                if (!string.IsNullOrWhiteSpace(jsonCredentials))
                {
                    var credential = GoogleCredential.FromJson(jsonCredentials);
                    _storageClient = StorageClient.Create(credential);
                }
                else
                {
                    _storageClient = StorageClient.Create();
                }
            }
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
                    contentType: string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                    source: stream
                );

                return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return;

            try
            {
                var uri = new Uri(fileUrl);
                var objectName = uri.AbsolutePath.TrimStart('/');

                if (objectName.StartsWith($"{_bucketName}/", StringComparison.OrdinalIgnoreCase))
                {
                    objectName = objectName.Substring(_bucketName.Length + 1);
                }

                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }
        }
    }
}