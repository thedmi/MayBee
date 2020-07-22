namespace MayBee.Serialization.JsonNet
{
    using Newtonsoft.Json;

    public static class JsonNetExtensions
    {
        public static JsonSerializerSettings ConfigureMaybe(this JsonSerializerSettings settings)
        {
            return ConfigureMaybe(settings, SerializationFormat.Nullable);
        }
        
        public static JsonSerializerSettings ConfigureMaybe(this JsonSerializerSettings settings, SerializationFormat format)
        {
            settings.Converters.Add(GetConverter(format));
            return settings;
        }
        
        public static JsonSerializer ConfigureMaybe(this JsonSerializer settings)
        {
            return ConfigureMaybe(settings, SerializationFormat.Nullable);
        }

        public static JsonSerializer ConfigureMaybe(this JsonSerializer serializer, SerializationFormat format)
        {
            serializer.Converters.Add(GetConverter(format));
            return serializer;
        }

        private static JsonConverter GetConverter(SerializationFormat format)
        {
            return format == SerializationFormat.Nullable
                ? (JsonConverter)new MaybeAsNullableConverter()
                : new MaybeAsArrayConverter();
        }
    }
}
