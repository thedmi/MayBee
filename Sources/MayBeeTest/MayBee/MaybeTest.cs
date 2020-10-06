// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaybeTest.cs" company="Dani Michel">
//   Dani Michel 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MayBeeTest.MayBee
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using global::MayBee;

    using Xunit;

#pragma warning disable 612
    public class MaybeTest
    {
        [Fact]
        public void EmptyMaybeWorksForValueTypes()
        {
            var maybe = Maybe.Empty<int>();

            Assert.True(maybe.IsEmpty);
            Assert.False(maybe.Exists);
            Assert.Equal(0, maybe.ItOrDefault);
            Assert.Throws<InvalidOperationException>(() => maybe.It);
            Assert.Throws<InvalidTimeZoneException>(() => maybe.ItOrThrow(new InvalidTimeZoneException()));
            Assert.Throws<InvalidTimeZoneException>(() => maybe.ItOrThrow(() => new InvalidTimeZoneException()));
            Assert.Null(maybe.AsNullable());
            Assert.Equal(0, maybe.AsList().Count);
        }

        [Fact]
        public void EmptyMaybeWorksForReferenceTypes()
        {
            var maybe = Maybe.Empty<string>();

            Assert.True(maybe.IsEmpty);
            Assert.False(maybe.Exists);
            Assert.Null(maybe.ItOrDefault);
            Assert.Throws<InvalidOperationException>(() => maybe.It);
            Assert.Throws<InvalidTimeZoneException>(() => maybe.ItOrThrow(new InvalidTimeZoneException()));
            Assert.Throws<InvalidTimeZoneException>(() => maybe.ItOrThrow(() => new InvalidTimeZoneException()));
            Assert.Equal(0, maybe.AsList().Count);
        }

        [Fact]
        public void ExistingMaybeWorksForValueTypes()
        {
            var maybe = Maybe.Is(34.2);

            Assert.False(maybe.IsEmpty);
            Assert.True(maybe.Exists);
            Assert.Equal(34.2, maybe.It);
            Assert.Equal(34.2, maybe.ItOrDefault);
            Assert.Equal(34.2, maybe.ItOrThrow(new InvalidTimeZoneException()));
            Assert.Equal(34.2, maybe.ItOrThrow(() => new InvalidTimeZoneException()));
            Assert.Equal(34.2, maybe.AsNullable());
            Assert.Equal(new[] { 34.2 } as IEnumerable<double>, maybe.AsList());
        }

        [Fact]
        public void ExistingMaybeWorksForReferenceTypes()
        {
            var maybe = Maybe.Is("Hello");

            Assert.False(maybe.IsEmpty);
            Assert.True(maybe.Exists);
            Assert.Equal("Hello", maybe.It);
            Assert.Equal("Hello", maybe.ItOrDefault);
            Assert.Equal("Hello", maybe.ItOrThrow(new InvalidTimeZoneException()));
            Assert.Equal("Hello", maybe.ItOrThrow(() => new InvalidTimeZoneException()));
            Assert.Equal(new[] { "Hello" } as IEnumerable<string>, maybe.AsList());
        }

        [Fact]
        public void CanConstructMaybeFromNullable()
        {
            var existing = Maybe.FromNullable("Hello");
            Assert.Equal("Hello", existing.It);

            var emptyRef = Maybe.FromNullable((string)null);
            Assert.True(emptyRef.IsEmpty);
            Assert.Null(emptyRef.ItOrDefault);

            var emptyStruct = Maybe.FromNullable((int?)null);
            Assert.True(emptyStruct.IsEmpty);
            Assert.Equal(0, emptyStruct.ItOrDefault);
        }

        [Fact]
        public void SelectMaintainsTheExistsOrEmptyState()
        {
            var lengthMaybe = Maybe.Is("Hello").Select(s => s.Length);

            Assert.True(lengthMaybe.Exists);
            Assert.Equal(5, lengthMaybe.It);

            var emptyLengthMaybe = Maybe.Empty<string>().Select(s => s.Length);

            Assert.False(emptyLengthMaybe.Exists);
        }

        [Fact]
        public void MonadicOperationsCompose()
        {
            Maybe<string> TryGetNumber(string s)
            {
                var match = Regex.Match(s, @"\d+");
                return match == Match.Empty ? Maybe.Empty<string>() : Maybe.Is(match.Value);
            }

            Assert.False(Maybe.Is("Hello   Foo Bar").SelectMany(TryGetNumber).Select(Int16.Parse).Select(n => n > 2).ItOrDefault);
            Assert.False(Maybe.Is("Hello 2 Foo Bar").SelectMany(TryGetNumber).Select(Int16.Parse).Select(n => n > 2).ItOrDefault);
            Assert.True(Maybe.Is("Hello 3 Foo Bar").SelectMany(TryGetNumber).Select(Int16.Parse).Select(n => n > 2).ItOrDefault);
        }

        [Fact]
        public void CanCastObjectMaybeToStringMaybe()
        {
            var maybe = Maybe.Is((object)"Hello").Cast<string>();

            Assert.True(maybe.Exists);
            Assert.Equal("Hello", maybe.It);

            var empty = Maybe.Empty<string>().Cast<object>();

            Assert.False(empty.Exists);
        }

        [Fact]
        public void CanProvideDefaultValue()
        {
            Assert.Equal("Hello", Maybe.Is("Hello").ItOr("Foo"));
            Assert.Equal("Foo", Maybe.Empty<string>().ItOr("Foo"));
        }

        [Fact]
        public void StringMaybeItOrEmptyReturnsCorrectValue()
        {
            Assert.Equal("Hello", Maybe.Is("Hello").ItOrEmpty());
            Assert.Equal("", Maybe.Empty<string>().ItOrEmpty());
        }

        [Fact]
        public void EqualityIsPassedThroughForExistingMaybes()
        {
            Assert.True(Maybe.Is(new Inner("one")) == Maybe.Is(new Inner("one")));
            Assert.False(Maybe.Is(new Inner("one")) == Maybe.Is(new Inner("two")));
        }
        
        [Fact]
        public void EmptyMaybesAreEqual()
        {
            Assert.True(Maybe.Empty<Inner>() == Maybe.Empty<Inner>());
        }
        
        [Fact]
        public void AnExistingAndInexistentMaybeAreNotEqual()
        {
            Assert.False(Maybe.Empty<Inner>() == Maybe.Is(new Inner("")));
        }

        private class Inner : IEquatable<Inner>
        {
            public Inner(string value)
            {
                Value = value;
            }

            public string Value { get; }

            public bool Equals(Inner other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Inner)obj);
            }

            public override int GetHashCode()
            {
                return (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
#pragma warning restore 612
}
