using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	internal static class StringUtils
	{
		public const string CarriageReturnLineFeed = "\r\n";

		public const string Empty = "";

		public const char CarriageReturn = '\r';

		public const char LineFeed = '\n';

		public const char Tab = '\t';

		public static StringWriter CreateStringWriter(int capacity)
		{
			return new StringWriter(new StringBuilder(capacity), CultureInfo.InvariantCulture);
		}

		public static bool EndsWith(this string source, char value)
		{
			if (source.Length <= 0)
			{
				return false;
			}
			return source[source.Length - 1] == value;
		}

		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> tSources = 
				from  in source
				where string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)
				select ;
			if (tSources.Count<TSource>() <= 1)
			{
				return tSources.SingleOrDefault<TSource>();
			}
			return (
				from  in source
				where string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)
				select ).SingleOrDefault<TSource>();
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0)
		{
			return format.FormatWith(provider, new object[] { arg0 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1, arg2 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1, arg2, arg3 });
		}

		private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool StartsWith(this string source, char value)
		{
			if (source.Length <= 0)
			{
				return false;
			}
			return source[0] == value;
		}

		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
			{
				return s;
			}
			char[] charArray = s.ToCharArray();
			int num = 0;
			while (true)
			{
				if (num < (int)charArray.Length)
				{
					if (num == 1 && !char.IsUpper(charArray[num]))
					{
						break;
					}
					if (!(num > 0 & num + 1 < (int)charArray.Length) || char.IsUpper(charArray[num + 1]))
					{
						charArray[num] = StringUtils.ToLower(charArray[num]);
						num++;
					}
					else
					{
						if (!char.IsSeparator(charArray[num + 1]))
						{
							break;
						}
						charArray[num] = StringUtils.ToLower(charArray[num]);
						break;
					}
				}
				else
				{
					break;
				}
			}
			return new string(charArray);
		}

		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex(c >> '\f' & 15);
			buffer[3] = MathUtils.IntToHex(c >> '\b' & 15);
			buffer[4] = MathUtils.IntToHex(c >> '\u0004' & 15);
			buffer[5] = MathUtils.IntToHex(c & '\u000F');
		}

		private static char ToLower(char c)
		{
			c = char.ToLower(c, CultureInfo.InvariantCulture);
			return c;
		}

		public static string ToSnakeCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringUtils.SnakeCaseState snakeCaseState = StringUtils.SnakeCaseState.Start;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ')
				{
					if (snakeCaseState != StringUtils.SnakeCaseState.Start)
					{
						snakeCaseState = StringUtils.SnakeCaseState.NewWord;
					}
				}
				else if (char.IsUpper(s[i]))
				{
					switch (snakeCaseState)
					{
						case StringUtils.SnakeCaseState.Lower:
						case StringUtils.SnakeCaseState.NewWord:
						{
							stringBuilder.Append('\u005F');
							break;
						}
						case StringUtils.SnakeCaseState.Upper:
						{
							if (!(i > 0 & i + 1 < s.Length))
							{
								break;
							}
							char chr = s[i + 1];
							if (char.IsUpper(chr) || chr == '\u005F')
							{
								break;
							}
							stringBuilder.Append('\u005F');
							break;
						}
					}
					char lower = char.ToLower(s[i], CultureInfo.InvariantCulture);
					stringBuilder.Append(lower);
					snakeCaseState = StringUtils.SnakeCaseState.Upper;
				}
				else if (s[i] != '\u005F')
				{
					if (snakeCaseState == StringUtils.SnakeCaseState.NewWord)
					{
						stringBuilder.Append('\u005F');
					}
					stringBuilder.Append(s[i]);
					snakeCaseState = StringUtils.SnakeCaseState.Lower;
				}
				else
				{
					stringBuilder.Append('\u005F');
					snakeCaseState = StringUtils.SnakeCaseState.Start;
				}
			}
			return stringBuilder.ToString();
		}

		public static string Trim(this string s, int start, int length)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = start + length - 1;
			if (num >= s.Length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			while (true)
			{
				if (start < num)
				{
					if (!char.IsWhiteSpace(s[start]))
					{
						break;
					}
					start++;
				}
				else
				{
					break;
				}
			}
			while (num >= start && char.IsWhiteSpace(s[num]))
			{
				num--;
			}
			return s.Substring(start, num - start + 1);
		}

		internal enum SnakeCaseState
		{
			Start,
			Lower,
			Upper,
			NewWord
		}
	}
}