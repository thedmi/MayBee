﻿namespace MayBeeTest.MayBee.Serialization.JsonNet
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using global::MayBee;
    using global::MayBee.Serialization.JsonNet;

    using Newtonsoft.Json;

    using Xunit;

    public class MaybeAsArrayConverterTest
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings().ConfigureMaybe();

        [Fact]
        public void Empty_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new TestRefType { TheMaybe = Maybe.Empty<string>() };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.IsEmpty);
        }

        [Fact]
        public void Existing_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new TestRefType { TheMaybe = Maybe.Is("Hello maybe!") };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.Exists);
            Assert.Equal("Hello maybe!", deserialized.TheMaybe.It);
        }

        [Fact]
        public void Empty_value_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new TestValueType { TheMaybe = Maybe.Empty<int>() };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.IsEmpty);
        }

        [Fact]
        public void Existing_value_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new TestValueType { TheMaybe = Maybe.Is(42) };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.Exists);
            Assert.Equal(42, deserialized.TheMaybe.It);
        }

        [Fact]
        public void Empty_boolean_maybes_roundtrip_sucessfully_to_json()
        {
            var testObj = new NestedType<bool> { TheMaybe = Maybe.Empty<bool>() };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.IsEmpty);
        }

        [Fact]
        public void Existing_boolean_maybes_roundtrip_sucessfully_to_json()
        {
            var testObj = new NestedType<bool> { TheMaybe = Maybe.Is(false) };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.Exists);
            Assert.False(deserialized.TheMaybe.It);
        }

        [Fact]
        public void Empty_nested_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new NestedType<InnerType> { TheMaybe = Maybe.Empty<InnerType>() };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.IsEmpty);
        }

        [Fact]
        public void Existing_nested_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new NestedType<InnerType> { TheMaybe = Maybe.Is(new InnerType { IntValue = 42, StringValue = "Heyo" }) };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.Exists);
            Assert.Equal(42, deserialized.TheMaybe.It.IntValue);
            Assert.Equal("Heyo", deserialized.TheMaybe.It.StringValue);
        }

        [Fact]
        public void Empty_array_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new NestedType<int[]> { TheMaybe = Maybe.Empty<int[]>() };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.IsEmpty);
        }

        [Fact]
        public void Existing_array_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new NestedType<int[]> { TheMaybe = Maybe.Is(new[] { 42, 43 }) };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.TheMaybe.Exists);
            Assert.Equal(2, deserialized.TheMaybe.It.Length);
            Assert.Equal(42, deserialized.TheMaybe.It[0]);
            Assert.Equal(43, deserialized.TheMaybe.It[1]);
        }

        private T PerformRoundtrip<T>(T testObj)
        {
            var json = JsonConvert.SerializeObject(testObj, _jsonSerializerSettings);

            Debug.WriteLine(json);

            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }

        [Fact]
        public void Existing_nested_array_maybe_roundtrips_successfully_to_json()
        {
            var testObj = new ArrayType<TestValueType>
                          {
                              Values =
                                  new[]
                                  {
                                      new TestValueType { TheMaybe = Maybe.Empty<int>() },
                                      new TestValueType { TheMaybe = Maybe.Is(1) }
                                  }
                          };

            var deserialized = PerformRoundtrip(testObj);

            Assert.True(deserialized.Values.Count == 2);
            Assert.True(deserialized.Values[0].TheMaybe.IsEmpty);
            Assert.Equal(1, deserialized.Values[1].TheMaybe.It);
        }

        private class TestRefType
        {
            public IMaybe<string> TheMaybe { get; set; }
        }

        private class TestValueType
        {
            public IMaybe<int> TheMaybe { get; set; }
        }

        private class NestedType<T>
        {
            public IMaybe<T> TheMaybe { get; set; }
        }

        private class InnerType
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
        }

        private class ArrayType<T>
        {
            public IReadOnlyList<T> Values { get; set; }
        }
    }
}
