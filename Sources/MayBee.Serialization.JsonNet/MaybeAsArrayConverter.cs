namespace MayBee.Serialization.JsonNet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MayBee;

    using Newtonsoft.Json;

    public class MaybeAsArrayConverter : JsonConverter
    {
        private static readonly Type _markerType = typeof(IMaybe);

        private static readonly MethodInfo _emptyCreatorMethod = typeof(Maybe).GetTypeInfo().GetDeclaredMethod("Empty");
        private static readonly MethodInfo _existingCreatorMethod = typeof(Maybe).GetTypeInfo().GetDeclaredMethod("Is");

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var maybe = (IMaybe)value;

            writer.WriteStartArray();
            if (maybe.Exists)
            {
                var innerValue = maybe.GetType().GetTypeInfo().GetDeclaredProperty("It").GetValue(maybe);
                serializer.Serialize(writer, innerValue);
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var nestedType = objectType.GenericTypeArguments.Single();

            var listType = typeof (IList<>).MakeGenericType(nestedType);
            var valueList = (IList)serializer.Deserialize(reader, listType);

            return valueList.Count == 0
                ? _emptyCreatorMethod.MakeGenericMethod(nestedType).Invoke(null, new object[] { })
                : _existingCreatorMethod.MakeGenericMethod(nestedType).Invoke(null, new[] { valueList[0] });
        }

        public override bool CanConvert(Type objectType)
        {
            return _markerType.GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}
