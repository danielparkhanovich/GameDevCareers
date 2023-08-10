using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Cache.Converters
{
    /// <summary>
    /// Just ignore it, don't save
    /// </summary>
    public class FormFileConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IFormFile);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, null);
        }
    }
}
