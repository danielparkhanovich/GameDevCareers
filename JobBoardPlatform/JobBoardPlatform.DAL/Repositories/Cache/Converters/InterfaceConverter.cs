using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JobBoardPlatform.DAL.Repositories.Cache.Converters
{
    public class InterfaceConverter<TInterface, TImplementation> : JsonConverter
        where TImplementation : TInterface
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TInterface);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = (TInterface)Activator.CreateInstance(typeof(TImplementation));
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(TImplementation));
        }
    }
}
