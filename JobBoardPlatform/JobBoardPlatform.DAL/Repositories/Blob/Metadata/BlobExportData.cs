
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob.Metadata
{
    public class BlobExportData
    {
        public IFormFile File { get; set; }
        public BlobHttpHeaders BlobHttpHeaders { get; set; } = new BlobHttpHeaders();
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
