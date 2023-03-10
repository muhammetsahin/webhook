using Newtonsoft.Json;

namespace Webhooks.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T data, Formatting formatting = Formatting.None, JsonSerializerSettings? settings = null)
        {
            JsonSerializer serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.CreateDefault(settings);
            serializer.Formatting = formatting;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, data, typeof(T));
                return sw.ToString();
            }
        }
        public static T FromJson<T>(this string data, JsonSerializerSettings? settings = null)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);

            JsonSerializer serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.CreateDefault(settings);

            using (var sw = new StringReader(data))
            using (var sr = new JsonTextReader(sw))
                return serializer.Deserialize<T>(sr);
        }
  }
}
