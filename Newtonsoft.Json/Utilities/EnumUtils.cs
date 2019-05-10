using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	internal static class EnumUtils
	{
		private readonly static ThreadSafeStore<StructMultiKey<Type, NamingStrategy>, EnumInfo> ValuesAndNamesPerEnum;

		private static CamelCaseNamingStrategy _camelCaseNamingStrategy;

		static EnumUtils()
		{
			Class6.yDnXvgqzyB5jw();
			EnumUtils.ValuesAndNamesPerEnum = new ThreadSafeStore<StructMultiKey<Type, NamingStrategy>, EnumInfo>(new Func<StructMultiKey<Type, NamingStrategy>, EnumInfo>(EnumUtils.InitializeValuesAndNames));
			EnumUtils._camelCaseNamingStrategy = new CamelCaseNamingStrategy();
		}

		private static int? FindIndexByName(string[] enumNames, string value, int valueIndex, int valueSubstringLength, StringComparison comparison)
		{
			for (int i = 0; i < (int)enumNames.Length; i++)
			{
				if (enumNames[i].Length == valueSubstringLength && string.Compare(enumNames[i], 0, value, valueIndex, valueSubstringLength, comparison) == 0)
				{
					return new int?(i);
				}
			}
			return null;
		}

		public static EnumInfo GetEnumValuesAndNames(Type enumType)
		{
			return EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, null));
		}

		public static IList<T> GetFlagsValues<T>(T value)
		where T : struct
		{
			Type type = typeof(T);
			if (!type.IsDefined(typeof(FlagsAttribute), false))
			{
				throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			ulong num = EnumUtils.ToUInt64(value);
			EnumInfo enumValuesAndNames = EnumUtils.GetEnumValuesAndNames(type);
			IList<T> ts = new List<T>();
			for (int i = 0; i < (int)enumValuesAndNames.Values.Length; i++)
			{
				ulong values = enumValuesAndNames.Values[i];
				if ((num & values) == values && values != 0)
				{
					ts.Add((T)Convert.ChangeType(values, underlyingType, CultureInfo.CurrentCulture));
				}
			}
			if (ts.Count == 0)
			{
				if (((IEnumerable<ulong>)enumValuesAndNames.Values).Any<ulong>((ulong v) => v == 0L))
				{
					ts.Add(default(T));
				}
			}
			return ts;
		}

		private static EnumInfo InitializeValuesAndNames(StructMultiKey<Type, NamingStrategy> key)
		{
			Type value1 = key.Value1;
			string[] names = Enum.GetNames(value1);
			string[] strArrays = new string[(int)names.Length];
			ulong[] num = new ulong[(int)names.Length];
			for (int i = 0; i < (int)names.Length; i++)
			{
				FieldInfo field = value1.GetField(names[i], BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				num[i] = EnumUtils.ToUInt64(field.GetValue(null));
				string str = (
					from EnumMemberAttribute a in field.GetCustomAttributes(typeof(EnumMemberAttribute), true)
					select a.Value).SingleOrDefault<string>() ?? field.Name;
				if (Array.IndexOf<string>(strArrays, str, 0, i) != -1)
				{
					throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.InvariantCulture, str, value1.Name));
				}
				strArrays[i] = (key.Value2 != null ? key.Value2.GetPropertyName(str, false) : str);
			}
			return new EnumInfo(value1.IsDefined(typeof(FlagsAttribute), false), num, names, strArrays);
		}

		private static string InternalFlagsFormat(EnumInfo entry, ulong result)
		{
			string str;
			string[] resolvedNames = entry.ResolvedNames;
			ulong[] values = entry.Values;
			int length = (int)values.Length - 1;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			ulong num = result;
			while (length >= 0 && (length != 0 || values[length] != 0))
			{
				if ((result & values[length]) == values[length])
				{
					result -= values[length];
					if (!flag)
					{
						stringBuilder.Insert(0, ", ");
					}
					stringBuilder.Insert(0, resolvedNames[length]);
					flag = false;
				}
				length--;
			}
			if (result != 0)
			{
				str = null;
			}
			else if (num != 0)
			{
				str = stringBuilder.ToString();
			}
			else if (values.Length == 0 || values[0] != 0)
			{
				str = null;
			}
			else
			{
				str = resolvedNames[0];
			}
			return str;
		}

		private static int? MatchName(string value, string[] enumNames, string[] resolvedNames, int valueIndex, int valueSubstringLength, StringComparison comparison)
		{
			int? nullable = EnumUtils.FindIndexByName(resolvedNames, value, valueIndex, valueSubstringLength, comparison);
			if (!nullable.HasValue)
			{
				nullable = EnumUtils.FindIndexByName(enumNames, value, valueIndex, valueSubstringLength, comparison);
			}
			return nullable;
		}

		public static object ParseEnum(Type enumType, NamingStrategy namingStrategy, string value, bool disallowNumber)
		{
			int length = 0;
			ValidationUtils.ArgumentNotNull(enumType, "enumType");
			ValidationUtils.ArgumentNotNull(value, "value");
			if (!enumType.IsEnum())
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			EnumInfo enumInfo = EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, namingStrategy));
			string[] names = enumInfo.Names;
			string[] resolvedNames = enumInfo.ResolvedNames;
			ulong[] values = enumInfo.Values;
			int? nullable = EnumUtils.FindIndexByName(resolvedNames, value, 0, value.Length, StringComparison.Ordinal);
			if (nullable.HasValue)
			{
				return Enum.ToObject(enumType, values[nullable.Value]);
			}
			int num = -1;
			int num1 = 0;
			while (true)
			{
				if (num1 >= value.Length)
				{
					break;
				}
				else if (!char.IsWhiteSpace(value[num1]))
				{
					num = num1;
					break;
				}
				else
				{
					num1++;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException("Must specify valid information for parsing in the string.");
			}
			char chr = value[num];
			if (char.IsDigit(chr) || chr == '-' || chr == '+')
			{
				Type underlyingType = Enum.GetUnderlyingType(enumType);
				value = value.Trim();
				object obj = null;
				try
				{
					obj = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
				}
				catch (FormatException formatException)
				{
				}
				if (obj != null)
				{
					if (disallowNumber)
					{
						throw new FormatException("Integer string '{0}' is not allowed.".FormatWith(CultureInfo.InvariantCulture, value));
					}
					return Enum.ToObject(enumType, obj);
				}
			}
			ulong num2 = 0L;
			for (int i = num; i <= value.Length; i = length + 1)
			{
				length = value.IndexOf(',', i);
				if (length == -1)
				{
					length = value.Length;
				}
				int num3 = length;
				while (i < length)
				{
					if (!char.IsWhiteSpace(value[i]))
					{
						break;
					}
					i++;
				}
				while (num3 > i && char.IsWhiteSpace(value[num3 - 1]))
				{
					num3--;
				}
				int num4 = num3 - i;
				nullable = EnumUtils.MatchName(value, names, resolvedNames, i, num4, StringComparison.Ordinal);
				if (!nullable.HasValue)
				{
					nullable = EnumUtils.MatchName(value, names, resolvedNames, i, num4, StringComparison.OrdinalIgnoreCase);
				}
				if (!nullable.HasValue)
				{
					nullable = EnumUtils.FindIndexByName(resolvedNames, value, 0, value.Length, StringComparison.OrdinalIgnoreCase);
					if (!nullable.HasValue)
					{
						throw new ArgumentException("Requested value '{0}' was not found.".FormatWith(CultureInfo.InvariantCulture, value));
					}
					return Enum.ToObject(enumType, values[nullable.Value]);
				}
				num2 |= values[nullable.Value];
			}
			return Enum.ToObject(enumType, num2);
		}

		private static ulong ToUInt64(object value)
		{
			bool flag;
			switch (ConvertUtils.GetTypeCode(value.GetType(), out flag))
			{
				case PrimitiveTypeCode.Char:
				{
					return (ulong)((char)value);
				}
				case PrimitiveTypeCode.CharNullable:
				case PrimitiveTypeCode.BooleanNullable:
				case PrimitiveTypeCode.SByteNullable:
				case PrimitiveTypeCode.Int16Nullable:
				case PrimitiveTypeCode.UInt16Nullable:
				case PrimitiveTypeCode.Int32Nullable:
				case PrimitiveTypeCode.ByteNullable:
				case PrimitiveTypeCode.UInt32Nullable:
				case PrimitiveTypeCode.Int64Nullable:
				{
					throw new InvalidOperationException("Unknown enum type.");
				}
				case PrimitiveTypeCode.Boolean:
				{
					return (ulong)Convert.ToByte((bool)value);
				}
				case PrimitiveTypeCode.SByte:
				{
					return (ulong)((sbyte)value);
				}
				case PrimitiveTypeCode.Int16:
				{
					return (ulong)((short)value);
				}
				case PrimitiveTypeCode.UInt16:
				{
					return (ulong)((ushort)value);
				}
				case PrimitiveTypeCode.Int32:
				{
					return (ulong)((int)value);
				}
				case PrimitiveTypeCode.Byte:
				{
					return (ulong)((byte)value);
				}
				case PrimitiveTypeCode.UInt32:
				{
					return (ulong)((uint)value);
				}
				case PrimitiveTypeCode.Int64:
				{
					return (ulong)value;
				}
				case PrimitiveTypeCode.UInt64:
				{
					return (ulong)value;
				}
				default:
				{
					throw new InvalidOperationException("Unknown enum type.");
				}
			}
		}

		public static bool TryToString(Type enumType, object value, bool camelCase, out string name)
		{
			NamingStrategy namingStrategy;
			Type type = enumType;
			object obj = value;
			if (camelCase)
			{
				namingStrategy = EnumUtils._camelCaseNamingStrategy;
			}
			else
			{
				namingStrategy = null;
			}
			return EnumUtils.TryToString(type, obj, namingStrategy, out name);
		}

		public static bool TryToString(Type enumType, object value, NamingStrategy namingStrategy, out string name)
		{
			EnumInfo enumInfo = EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, namingStrategy));
			ulong num = EnumUtils.ToUInt64(value);
			if (enumInfo.IsFlags)
			{
				name = EnumUtils.InternalFlagsFormat(enumInfo, num);
				return name != null;
			}
			int num1 = Array.BinarySearch<ulong>(enumInfo.Values, num);
			if (num1 < 0)
			{
				name = null;
				return false;
			}
			name = enumInfo.ResolvedNames[num1];
			return true;
		}
	}
}