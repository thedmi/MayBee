namespace Maybe.Serialization.JsonNet
{
    using Newtonsoft.Json;

    public static class JsonNetExtensions
    {
        public static JsonSerializerSettings ConfigureMaybe(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new MaybeConverter());

            return settings;
        }

        public static JsonSerializer ConfigureMaybe(this JsonSerializer serializer)
        {
            serializer.Converters.Add(new MaybeConverter());

            return serializer;
        }
    }
}
