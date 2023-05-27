using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public abstract class CacheRepositoryCore<T> : ICacheRepository<T>
    {
        private readonly IDistributedCache cache;


        public CacheRepositoryCore(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task UpdateAsync(T entry)
        {
            var serialized = GetSerialized(entry);
            var bytes = TryGetBytesFromSerialized(serialized);
            await cache.SetAsync(GetEntryKey(), bytes, GetOptions());
        }

        public async Task<T> GetAsync()
        {
            var bytes = await cache.GetAsync(GetEntryKey());
            var serialized = TrySerializedFromBytes(bytes);
            var entry = TryDeserializeEntry(serialized);
            return entry!;
        }

        protected abstract string GetEntryKey();

        protected abstract DistributedCacheEntryOptions GetOptions();

        private string GetSerialized(T entry)
        {
            return JsonConvert.SerializeObject(entry);
        }

        private byte[] TryGetBytesFromSerialized(string serialized)
        {
            if (IsSerializedEmpty(serialized))
            {
                throw new Exception(
                    $"Unable to update cache. " +
                    $"Forbidden to update to empty state. " +
                    $"Cannot get bytes from: {serialized}");
            }
            var bytes = GetBytesFromSerialized(serialized);
            return bytes;
        }

        private byte[] GetBytesFromSerialized(string serialized)
        {
            return Encoding.UTF8.GetBytes(serialized);
        }

        private string TrySerializedFromBytes(byte[]? bytes)
        {
            if (bytes == null)
            {
                throw new Exception(
                    $"Unable to get data from the cache. " +
                    $"Received data is empty. " +
                    $"Cannot get serialized from: {bytes}");
            }
            var serialized = GetSerializedFromBytes(bytes);
            return serialized;
        }

        private string GetSerializedFromBytes(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        private T TryDeserializeEntry(string serialized)
        {
            var entry = JsonConvert.DeserializeObject<T>(serialized);
            if (entry == null)
            {
                throw new Exception(
                    $"Unable to get data from the cache. " +
                    $"Deserialization exception.");
            }
            return entry;
        }

        private bool IsSerializedEmpty(string serialized)
        {
            return string.IsNullOrEmpty(serialized) || serialized == "[]";
        }
    }
}
