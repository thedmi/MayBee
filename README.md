MayBee - A Lightweight Wrapper Type for Optional Values (aka Maybes) in .NET
============================================================================

The `Maybe<T>` class provides a type-safe way to express optional values or relations. Conversion and projection methods enable seamless integration with libraries that represent optional values as `null`. The `IMaybe<out T>` interface provides covariance where needed.

Grab the NuGet [here](https://www.nuget.org/packages/MayBee/).


Motivation
-----------

Accepting `null` as a value that denotes "empty" for optional values has several drawbacks, the most important being the lack of type safety: The type system cannot distinguish between optional and required values, because both have the same type (except for `struct`, where we have `Nullable` to denote optional values). However, the information whether e.g. a relation is optional or required is important information, especially with domain modeling.

If we cannot use null for optional things like 0..1 relations, we need something else. Enter `Maybe<T>`.

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


### Monadic Operations

Operating on maybes is afforded by LINQ with `Select` and `SelectMany` providing map and flatMap functionality, respectively.

Given a `client` with a `LoyaltyAccount` property returning a `Maybe<LoyaltyAccount>`, and with `LoyaltyAccount` having a `Points` property, we can send emails to *only* clients having a loyalty account with more than 100 points:

```
if(client.LoyaltyAccount.Select(a => a.Points > 100).ItOrDefault) {
	// send email
}
```

If we further imagine that loyalty accounts may be linked to special discount rules (i.e. the `SpecialDiscounts` will return a maybe), we can send email to only clients with a special discount on books:

```
if(client.LoyaltyAccount.SelectMany(a => a.SpecialDiscounts).Select(d => d.HasBookDiscount).ItOrDefault) {
	// send email about book discount
}
```