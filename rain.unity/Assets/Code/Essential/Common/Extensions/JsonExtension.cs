using System.Collections.Generic;
using Essential.Common.Conversion.Json;
using Newtonsoft.Json;

namespace Essential.Common.Extensions
{
    public static class JsonExtension
    {
        private static JsonSerializerSettings _jsonSettings { get; } = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new ReactiveFloatJsonConverter(),
                new ReactiveIntJsonConverter()
            }
        };

        public static string AsJson<T>(this T data)
        {
            return JsonConvert.SerializeObject(data, _jsonSettings);
        }

        public static T AsData<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }
    }
}