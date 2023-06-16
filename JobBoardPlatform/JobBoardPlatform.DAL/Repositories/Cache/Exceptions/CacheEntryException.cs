
namespace JobBoardPlatform.BLL.Services.Session.Tokens
{
    public class CacheEntryException : Exception
    {
        public CacheEntryException()
        {
        }

        public CacheEntryException(string message)
            : base(message)
        {
        }

        public CacheEntryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public static CacheEntryException UnableToUpdateEntry(string serialized)
        {
            return new CacheEntryException(
                    $"Unable to update cache. " +
                    $"Forbidden to update to empty state. " +
                    $"Cannot get bytes from: {serialized}");
        }

        public static CacheEntryException UnableToGetDataEntry(byte[]? bytes)
        {
            return new CacheEntryException(
                    $"Unable to get data from the cache. " +
                    $"Received data is empty. " +
                    $"Cannot get serialized from: {bytes}");
        }

        public static CacheEntryException UnableToGetEntryDeserialization()
        {
            return new CacheEntryException(
                    $"Unable to get data from the cache. " +
                    $"Deserialization exception.");
        }
    }
}
