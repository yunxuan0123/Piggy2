using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	public static class Extensions
	{
		public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source)
		where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.Ancestors()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> AncestorsAndSelf<T>(this IEnumerable<T> source)
		where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.AncestorsAndSelf()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source)
		where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			IJEnumerable<T> ts = source as IJEnumerable<T>;
			IJEnumerable<T> ts1 = ts;
			if (ts != null)
			{
				return ts1;
			}
			return new JEnumerable<T>(source);
		}

		public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source)
		where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source)
		where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T c) => c.Children()).Convert<JToken, U>();
		}

		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source)
		where T : JToken
		{
			return new Extensions.<Convert>d__14<T, U>(-2)
			{
				<>3__source = source
			};
		}

		internal static U Convert<T, U>(this T token)
		where T : JToken
		{
			U u;
			if (token == null)
			{
				u = default(U);
				return u;
			}
			T t = token;
			T t1 = t;
			if ((object)t is U)
			{
				U u1 = (U)(object)t1;
				if (typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
				{
					return u1;
				}
			}
			JValue jValue = (object)token as JValue;
			JValue jValue1 = jValue;
			if (jValue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
			}
			object value = jValue1.Value;
			object obj = value;
			if (value is U)
			{
				return (U)obj;
			}
			Type underlyingType = typeof(U);
			if (ReflectionUtils.IsNullableType(underlyingType))
			{
				if (jValue1.Value == null)
				{
					u = default(U);
					return u;
				}
				underlyingType = Nullable.GetUnderlyingType(underlyingType);
			}
			return (U)Convert.ChangeType(jValue1.Value, underlyingType, CultureInfo.InvariantCulture);
		}

		public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source)
		where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.Descendants()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> DescendantsAndSelf<T>(this IEnumerable<T> source)
		where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.DescendantsAndSelf()).AsJEnumerable();
		}

		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<JObject, JProperty>((JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		public static U Value<T, U>(this IEnumerable<T> value)
		where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jTokens = value as JToken;
			if (jTokens == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jTokens.Convert<JToken, U>();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
		{
			return Extensions.Values<JToken, JToken>(source, key).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
		{
			return Extensions.Values<JToken, U>(source, key);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
		{
			return Extensions.Values<JToken, U>(source, null);
		}

		internal static IEnumerable<U> Values<T, U>(IEnumerable<T> source, object key)
		where T : JToken
		{
			return new Extensions.<Values>d__11<T, U>(-2)
			{
				<>3__source = source,
				<>3__key = key
			};
		}
	}
}