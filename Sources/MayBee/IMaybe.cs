// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaybe.cs" company="Dani Michel">
//   Dani Michel 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MayBee
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents values of type <typeparamref name="T"/> that may or may not exist.
    /// </summary>
    public interface IMaybe<T> : IMaybe
    {
        /// <summary>
        /// Returns the actual value of the Maybe and throws if it is empty.
        /// </summary>
        T It { get; }

        /// <summary>
        /// Returns the value of the Maybe if it exists, or default(T) otherwise. 
        /// </summary>
        T ItOrDefault { get; }

        /// <summary>
        /// Returns the value of the Maybe if it exists, or <paramref name="defaultValue"/> otherwise. 
        /// </summary>
        T ItOr(T defaultValue);

        /// <summary>
        /// Returns <see cref="It"/> or throws <paramref name="exception"/> if it doesn't exist.
        /// </summary>
        [Obsolete]
        T ItOrThrow(Exception exception);

        /// <summary>
        /// Returns <see cref="It"/> or throws the exception created by <paramref name="exceptionCreator"/> if it doesn't exist.
        /// </summary>
        T ItOrThrow(Func<Exception> exceptionCreator);

        /// <summary>
        /// Returns the maybe as read-only list of T. This list has the length of one if the maybe
        /// exists, or zero if the maybe is empty.
        /// </summary>
        /// <returns>An immutable list of length one or zero.</returns>
        IReadOnlyList<T> AsList();
    }

    public interface IMaybe
    {
        /// <summary>
        /// Returns true if there is no value in this maybe.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns true if a value exists and accessing it is safe.
        /// </summary>
        bool Exists { get; }
    }
}