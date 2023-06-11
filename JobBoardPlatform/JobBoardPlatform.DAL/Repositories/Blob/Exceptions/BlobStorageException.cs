
namespace JobBoardPlatform.DAL.Repositories.Blob.Exceptions
{
    public class BlobStorageException : Exception
    {
        public const string ItemNotFound = "Item not found";


        public BlobStorageException()
        {
        }

        public BlobStorageException(string message)
            : base(message)
        {
        }

        public BlobStorageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
