namespace MayBee.Serialization.JsonNet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;

    public class MaybeAsNullableConverter : JsonConverter
    {
        private static readonly Type _markerType = typeof(IMaybe);

        private static readonly MethodInfo _emptyCreatorMethod = typeof(Maybe).GetTypeInfo().GetDeclaredMethod("Empty");
        private static readonly MethodInfo _existingCreatorMethod = typeof(Maybe).GetTypeInfo().GetDeclaredMethod("Is");

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var maybe = (IMaybe)value;

            if (maybe.Exists)
            {
                var innerValue = maybe.GetType().GetTypeInfo().GetDeclaredProperty("It").GetValue(maybe);
                serializer.Serialize(writer, innerValue);
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var innerMaybeType = objectType.GenericTypeArguments.Single();

            // For value types, we must deserialize into the nullable variant, otherwise deserializing 'null' will fail
            var deserializationType = innerMaybeType.IsValueType
                ? typeof(Nullable<>).MakeGenericType(innerMaybeType)
                : innerMaybeType;
            
            var innerValue = serializer.Deserialize(reader, deserializationType);

            return innerValue == null
                ? _emptyCreatorMethod.MakeGenericMethod(innerMaybeType).Invoke(null, new object[] { })
                : _existingCreatorMethod.MakeGenericMethod(innerMaybeType).Invoke(null, new[] { innerValue });
        }

        public override bool CanConvert(Type objectType)
        {
            return _markerType.GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}
