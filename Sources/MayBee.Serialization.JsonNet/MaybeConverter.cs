namespace MayBee.Serialization.JsonNet
{
    using System;

    using Newtonsoft.Json;

    [Obsolete("Use MaybeAsArrayConverter instead for backwards compatibility, or switch to MaybeAsNullableConverter if desired (not compatible)")]
    public class MaybeConverter : JsonConverter
    {
        private readonly MaybeAsArrayConverter _arrayConverter = new MaybeAsArrayConverter();

        public override bool CanRead => _arrayConverter.CanRead;

        public override bool CanWrite => _arrayConverter.CanWrite;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            _arrayConverter.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return _arrayConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override bool CanConvert(Type objectType)
        {
            return _arrayConverter.CanConvert(objectType);
        }
    }
}
