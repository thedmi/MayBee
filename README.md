Maybe - A Lightweight Wrapper Type for Optional Values in .NET
===============================================================

The `Maybe<T>` class provides a type-safe way to express optional values or relations. Conversion and projection methods enable seamless integration with libraries that represent optional values as `null`. The `IMaybe<out T>` interface provides covariance where needed.


Motivation
-----------

In my programs, `null` is never, ever a valid value and always considered a bug. I employ this strict rule for the sake of readability, because this way

- I never have to check actual parameters for null, since calling the method with null would be an error of the caller.
- There is no implicit, hidden or incomprehensible notion of a non-existing value that would be a valid method parameter to mean "nothing" or "empty" or the like.

If we cannot use null for optional things like 0..1 relations, we need something else. Enter `Maybe<T>`. You will never ask yourself again "do I need a null check here?".

Of course there are situations when you need to use `null`, for example if a library expects it. For that case, the Maybe offers conversion methods, which again make the intention clear.


Usage
------

### Instantiation

To create an existing value wrapped in a maybe, use the constructor `Maybe(T value)` or the static creator `Maybe.Is(T value)`. To create an empty instance, use the default constructor or the static creator `Maybe.Empty<T>()`.


### Unwrapping

Just use one of the `It...` properties to get the original value out of the maybe. There are overloads that throw if the maybe is empty, and one that returns `default(T)`. The `AsList()` method returns a list of length one (value exists) or zero (maybe is empty).


### Conversion to/from null

Use `Maybe.FromNullable()` to wrap a value in a maybe, creating an empty one if the value is `null`, or an existing one if it isn't. There are two overloads, one for reference types, one for nullable value types. 

Similarly, use the `AsNullable()` extension method to retrieve the value as nullable value-type. For reference-typed maybes, `ItOrDefault` is the way to go, since `default(T) == null` for reference types.




