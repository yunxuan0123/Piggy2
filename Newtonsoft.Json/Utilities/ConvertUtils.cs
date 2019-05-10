using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal static class ConvertUtils
	{
		private readonly static Dictionary<Type, PrimitiveTypeCode> TypeCodeMap;

		private readonly static TypeInformation[] PrimitiveTypeCodes;

		private readonly static ThreadSafeStore<StructMultiKey<Type, Type>, Func<object, object>> CastConverters;

		static ConvertUtils()
		{
			Class6.yDnXvgqzyB5jw();
			ConvertUtils.TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>()
			{
				{ typeof(char), PrimitiveTypeCode.Char },
				{ typeof(char?), PrimitiveTypeCode.CharNullable },
				{ typeof(bool), PrimitiveTypeCode.Boolean },
				{ typeof(bool?), PrimitiveTypeCode.BooleanNullable },
				{ typeof(sbyte), PrimitiveTypeCode.SByte },
				{ typeof(sbyte?), PrimitiveTypeCode.SByteNullable },
				{ typeof(short), PrimitiveTypeCode.Int16 },
				{ typeof(short?), PrimitiveTypeCode.Int16Nullable },
				{ typeof(ushort), PrimitiveTypeCode.UInt16 },
				{ typeof(ushort?), PrimitiveTypeCode.UInt16Nullable },
				{ typeof(int), PrimitiveTypeCode.Int32 },
				{ typeof(int?), PrimitiveTypeCode.Int32Nullable },
				{ typeof(byte), PrimitiveTypeCode.Byte },
				{ typeof(byte?), PrimitiveTypeCode.ByteNullable },
				{ typeof(uint), PrimitiveTypeCode.UInt32 },
				{ typeof(uint?), PrimitiveTypeCode.UInt32Nullable },
				{ typeof(long), PrimitiveTypeCode.Int64 },
				{ typeof(long?), PrimitiveTypeCode.Int64Nullable },
				{ typeof(ulong), PrimitiveTypeCode.UInt64 },
				{ typeof(ulong?), PrimitiveTypeCode.UInt64Nullable },
				{ typeof(float), PrimitiveTypeCode.Single },
				{ typeof(float?), PrimitiveTypeCode.SingleNullable },
				{ typeof(double), PrimitiveTypeCode.Double },
				{ typeof(double?), PrimitiveTypeCode.DoubleNullable },
				{ typeof(DateTime), PrimitiveTypeCode.DateTime },
				{ typeof(DateTime?), PrimitiveTypeCode.DateTimeNullable },
				{ typeof(DateTimeOffset), PrimitiveTypeCode.DateTimeOffset },
				{ typeof(DateTimeOffset?), PrimitiveTypeCode.DateTimeOffsetNullable },
				{ typeof(decimal), PrimitiveTypeCode.Decimal },
				{ typeof(decimal?), PrimitiveTypeCode.DecimalNullable },
				{ typeof(Guid), PrimitiveTypeCode.Guid },
				{ typeof(Guid?), PrimitiveTypeCode.GuidNullable },
				{ typeof(TimeSpan), PrimitiveTypeCode.TimeSpan },
				{ typeof(TimeSpan?), PrimitiveTypeCode.TimeSpanNullable },
				{ typeof(BigInteger), PrimitiveTypeCode.BigInteger },
				{ typeof(BigInteger?), PrimitiveTypeCode.BigIntegerNullable },
				{ typeof(Uri), PrimitiveTypeCode.Uri },
				{ typeof(string), PrimitiveTypeCode.String },
				{ typeof(byte[]), PrimitiveTypeCode.Bytes },
				{ typeof(DBNull), PrimitiveTypeCode.DBNull }
			};
			ConvertUtils.PrimitiveTypeCodes = new TypeInformation[] { new TypeInformation()
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			}, new TypeInformation()
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Object
			}, new TypeInformation()
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.DBNull
			}, new TypeInformation()
			{
				Type = typeof(bool),
				TypeCode = PrimitiveTypeCode.Boolean
			}, new TypeInformation()
			{
				Type = typeof(char),
				TypeCode = PrimitiveTypeCode.Char
			}, new TypeInformation()
			{
				Type = typeof(sbyte),
				TypeCode = PrimitiveTypeCode.SByte
			}, new TypeInformation()
			{
				Type = typeof(byte),
				TypeCode = PrimitiveTypeCode.Byte
			}, new TypeInformation()
			{
				Type = typeof(short),
				TypeCode = PrimitiveTypeCode.Int16
			}, new TypeInformation()
			{
				Type = typeof(ushort),
				TypeCode = PrimitiveTypeCode.UInt16
			}, new TypeInformation()
			{
				Type = typeof(int),
				TypeCode = PrimitiveTypeCode.Int32
			}, new TypeInformation()
			{
				Type = typeof(uint),
				TypeCode = PrimitiveTypeCode.UInt32
			}, new TypeInformation()
			{
				Type = typeof(long),
				TypeCode = PrimitiveTypeCode.Int64
			}, new TypeInformation()
			{
				Type = typeof(ulong),
				TypeCode = PrimitiveTypeCode.UInt64
			}, new TypeInformation()
			{
				Type = typeof(float),
				TypeCode = PrimitiveTypeCode.Single
			}, new TypeInformation()
			{
				Type = typeof(double),
				TypeCode = PrimitiveTypeCode.Double
			}, new TypeInformation()
			{
				Type = typeof(decimal),
				TypeCode = PrimitiveTypeCode.Decimal
			}, new TypeInformation()
			{
				Type = typeof(DateTime),
				TypeCode = PrimitiveTypeCode.DateTime
			}, new TypeInformation()
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			}, new TypeInformation()
			{
				Type = typeof(string),
				TypeCode = PrimitiveTypeCode.String
			} };
			ConvertUtils.CastConverters = new ThreadSafeStore<StructMultiKey<Type, Type>, Func<object, object>>(new Func<StructMultiKey<Type, Type>, Func<object, object>>(ConvertUtils.CreateCastConverter));
		}

		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			object obj;
			switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out obj))
			{
				case ConvertUtils.ConvertResult.Success:
				{
					return obj;
				}
				case ConvertUtils.ConvertResult.CannotConvertNull:
				{
					throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
				}
				case ConvertUtils.ConvertResult.NotInstantiableType:
				{
					throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, targetType), "targetType");
				}
				case ConvertUtils.ConvertResult.NoValidConversion:
				{
					throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
				}
				default:
				{
					throw new InvalidOperationException("Unexpected conversion result.");
				}
			}
		}

		public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
		{
			object obj;
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			if (ConvertUtils.TryConvert(initialValue, culture, targetType, out obj))
			{
				return obj;
			}
			return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		private static Func<object, object> CreateCastConverter(StructMultiKey<Type, Type> t)
		{
			Type value1 = t.Value1;
			Type value2 = t.Value2;
			MethodInfo method = value2.GetMethod("op_Implicit", new Type[] { value1 }) ?? value2.GetMethod("op_Explicit", new Type[] { value1 });
			if (method == null)
			{
				return null;
			}
			MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => methodCall(null, new object[] { o });
		}

		public static ParseResult DecimalTryParse(char[] chars, int start, int length, out decimal value)
		{
			char? nullable;
			int? nullable1;
			int? nullable2;
			bool valueOrDefault;
			bool flag;
			int? nullable3;
			int? nullable4;
			value = new decimal();
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag1 = chars[start] == '-';
			bool flag2 = flag1;
			if (flag1)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start;
			int num1 = start + length;
			int num2 = num1;
			int num3 = num1;
			int num4 = 0;
			ulong num5 = 0L;
			ulong num6 = 0L;
			int num7 = 0;
			int num8 = 0;
			char? nullable5 = null;
			bool? nullable6 = null;
			while (num < num1)
			{
				char chr = chars[num];
				if (chr != '.')
				{
					if (chr != 'E' && chr != 'e')
					{
						if (chr < '0' || chr > '9')
						{
							return ParseResult.Invalid;
						}
						if (num == start && chr == '0')
						{
							num++;
							if (num == num1)
							{
								goto Label3;
							}
							chr = chars[num];
							if (chr == '.')
							{
								goto Label0;
							}
							if (chr != 'e' && chr != 'E')
							{
								return ParseResult.Invalid;
							}
							else
							{
								goto Label2;
							}
						}
					Label3:
						if (num7 < 29)
						{
							if (num7 == 28)
							{
								bool? nullable7 = nullable6;
								if (nullable7.HasValue)
								{
									valueOrDefault = nullable7.GetValueOrDefault();
								}
								else
								{
									if (num5 > 7922816251426433759L)
									{
										flag = true;
									}
									else if (num5 != 7922816251426433759L)
									{
										flag = false;
									}
									else if (num6 > 354395033L)
									{
										flag = true;
									}
									else
									{
										flag = (num6 != 354395033L ? false : chr > '5');
									}
									nullable6 = new bool?(flag);
									valueOrDefault = nullable6.GetValueOrDefault();
								}
								if (valueOrDefault)
								{
									goto Label4;
								}
							}
							if (num7 >= 19)
							{
								num6 = num6 * 10L + (long)(chr - 48);
							}
							else
							{
								num5 = num5 * 10L + (long)(chr - 48);
							}
							num7++;
							goto Label1;
						}
					Label4:
						if (!nullable5.HasValue)
						{
							nullable5 = new char?(chr);
						}
						num8++;
						goto Label1;
					}
				Label2:
					if (num == start)
					{
						return ParseResult.Invalid;
					}
					if (num == num2)
					{
						return ParseResult.Invalid;
					}
					num++;
					if (num == num1)
					{
						return ParseResult.Invalid;
					}
					if (num2 < num1)
					{
						num3 = num - 1;
					}
					chr = chars[num];
					bool flag3 = false;
					if (chr == '+')
					{
						num++;
					}
					else if (chr == '-')
					{
						flag3 = true;
						num++;
					}
					while (num < num1)
					{
						chr = chars[num];
						if (chr < '0' || chr > '9')
						{
							return ParseResult.Invalid;
						}
						int num9 = 10 * num4 + (chr - 48);
						if (num4 < num9)
						{
							num4 = num9;
						}
						num++;
					}
					if (flag3)
					{
						num4 = -num4;
					}
				}
				else
				{
					goto Label0;
				}
			Label1:
				num++;
			}
			num4 += num8;
			num4 = num4 - (num3 - num2);
			if (num7 > 19)
			{
				value = (num5 / new decimal(1, 0, 0, false, (byte)(num7 - 19))) + num6;
			}
			else
			{
				value = num5;
			}
			if (num4 <= 0)
			{
				nullable = nullable5;
				if (nullable.HasValue)
				{
					nullable3 = new int?(nullable.GetValueOrDefault());
				}
				else
				{
					nullable1 = null;
					nullable3 = nullable1;
				}
				nullable2 = nullable3;
				if (nullable2.GetValueOrDefault() >= 53 & nullable2.HasValue && num4 >= -28)
				{
					value = value++;
				}
				if (num4 < 0)
				{
					if (num7 + num4 + 28 <= 0)
					{
						value = (flag2 ? decimal.Zero : decimal.Zero);
						return ParseResult.Success;
					}
					if (num4 < -28)
					{
						value /= new decimal(268435456, 1042612833, 542101086, false, 0);
						value *= new decimal(1, 0, 0, false, (byte)(-num4 - 28));
					}
					else
					{
						value *= new decimal(1, 0, 0, false, (byte)(-num4));
					}
				}
			}
			else
			{
				num7 += num4;
				if (num7 > 29)
				{
					return ParseResult.Overflow;
				}
				if (num7 != 29)
				{
					value /= new decimal(1, 0, 0, false, (byte)num4);
				}
				else
				{
					if (num4 > 1)
					{
						value /= new decimal(1, 0, 0, false, (byte)(num4 - 1));
						if (value > new decimal(-1717986919, -1717986919, 429496729, false, 0))
						{
							return ParseResult.Overflow;
						}
					}
					else if (value == new decimal(-1717986919, -1717986919, 429496729, false, 0))
					{
						nullable = nullable5;
						if (nullable.HasValue)
						{
							nullable4 = new int?(nullable.GetValueOrDefault());
						}
						else
						{
							nullable1 = null;
							nullable4 = nullable1;
						}
						nullable2 = nullable4;
						if (nullable2.GetValueOrDefault() > 53 & nullable2.HasValue)
						{
							return ParseResult.Overflow;
						}
					}
					value *= new decimal(10);
				}
			}
			if (flag2)
			{
				value = -value;
			}
			return ParseResult.Success;
		Label0:
			if (num == start)
			{
				return ParseResult.Invalid;
			}
			if (num + 1 == num1)
			{
				return ParseResult.Invalid;
			}
			if (num2 != num1)
			{
				return ParseResult.Invalid;
			}
			num2 = num + 1;
			goto Label1;
		}

		private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
		{
			Type type;
			object str;
			if (value != null)
			{
				type = value.GetType();
			}
			else
			{
				type = null;
			}
			Type type1 = type;
			if (value != null)
			{
				if (targetType.IsAssignableFrom(type1))
				{
					return value;
				}
				Func<object, object> func = ConvertUtils.CastConverters.Get(new StructMultiKey<Type, Type>(type1, targetType));
				if (func != null)
				{
					return func(value);
				}
			}
			else if (ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			if (initialType != null)
			{
				str = initialType.ToString();
				if (str != null)
				{
					throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(invariantCulture, str, targetType));
				}
			}
			else
			{
				str = null;
			}
			str = "{null}";
			throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(invariantCulture, str, targetType));
		}

		public static object FromBigInteger(BigInteger i, Type targetType)
		{
			object obj;
			if (targetType == typeof(decimal))
			{
				return (decimal)i;
			}
			if (targetType == typeof(double))
			{
				return (double)((double)i);
			}
			if (targetType == typeof(float))
			{
				return (float)((float)i);
			}
			if (targetType == typeof(ulong))
			{
				return (ulong)i;
			}
			if (targetType == typeof(bool))
			{
				return i != 0L;
			}
			try
			{
				obj = Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), exception);
			}
			return obj;
		}

		public static PrimitiveTypeCode GetTypeCode(Type t)
		{
			bool flag;
			return ConvertUtils.GetTypeCode(t, out flag);
		}

		public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
		{
			PrimitiveTypeCode primitiveTypeCode;
			if (ConvertUtils.TypeCodeMap.TryGetValue(t, out primitiveTypeCode))
			{
				isEnum = false;
				return primitiveTypeCode;
			}
			if (t.IsEnum())
			{
				isEnum = true;
				return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
			}
			if (ReflectionUtils.IsNullableType(t))
			{
				Type underlyingType = Nullable.GetUnderlyingType(t);
				if (underlyingType.IsEnum())
				{
					Type type = typeof(Nullable<>);
					Type[] typeArray = new Type[] { Enum.GetUnderlyingType(underlyingType) };
					isEnum = true;
					return ConvertUtils.GetTypeCode(type.MakeGenericType(typeArray));
				}
			}
			isEnum = false;
			return PrimitiveTypeCode.Object;
		}

		public static TypeInformation GetTypeInformation(IConvertible convertable)
		{
			return ConvertUtils.PrimitiveTypeCodes[(int)convertable.GetTypeCode()];
		}

		public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
		{
			value = 0;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			bool flag1 = flag;
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			if (length > 10 || length == 10 && chars[start] - 48 > '\u0002')
			{
				for (int i = start; i < num; i++)
				{
					int num1 = chars[i] - 48;
					if (num1 < 0 || num1 > 9)
					{
						return ParseResult.Invalid;
					}
				}
				return ParseResult.Overflow;
			}
			for (int j = start; j < num; j++)
			{
				int num2 = chars[j] - 48;
				if (num2 < 0 || num2 > 9)
				{
					return ParseResult.Invalid;
				}
				int num3 = 10 * value - num2;
				if (num3 > value)
				{
					for (j++; j < num; j++)
					{
						num2 = chars[j] - 48;
						if (num2 < 0 || num2 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num3;
			}
			if (!flag1)
			{
				if (value == -2147483648)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
		{
			value = 0L;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			bool flag1 = flag;
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			if (length > 19)
			{
				for (int i = start; i < num; i++)
				{
					int num1 = chars[i] - 48;
					if (num1 < 0 || num1 > 9)
					{
						return ParseResult.Invalid;
					}
				}
				return ParseResult.Overflow;
			}
			for (int j = start; j < num; j++)
			{
				int num2 = chars[j] - 48;
				if (num2 < 0 || num2 > 9)
				{
					return ParseResult.Invalid;
				}
				long num3 = 10L * value - (long)num2;
				if (num3 > value)
				{
					for (j++; j < num; j++)
					{
						num2 = chars[j] - 48;
						if (num2 < 0 || num2 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num3;
			}
			if (!flag1)
			{
				if (value == -9223372036854775808L)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		public static bool IsConvertible(Type t)
		{
			return typeof(IConvertible).IsAssignableFrom(t);
		}

		public static bool IsInteger(object value)
		{
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
				case PrimitiveTypeCode.SByte:
				case PrimitiveTypeCode.Int16:
				case PrimitiveTypeCode.UInt16:
				case PrimitiveTypeCode.Int32:
				case PrimitiveTypeCode.Byte:
				case PrimitiveTypeCode.UInt32:
				case PrimitiveTypeCode.Int64:
				case PrimitiveTypeCode.UInt64:
				{
					return true;
				}
				default:
				{
					return false;
				}
			}
		}

		public static TimeSpan ParseTimeSpan(string input)
		{
			return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
		}

		internal static BigInteger ToBigInteger(object value)
		{
			object obj = value;
			object obj1 = obj;
			if (obj is BigInteger)
			{
				return (BigInteger)obj1;
			}
			string str = value as string;
			string str1 = str;
			if (str != null)
			{
				return BigInteger.Parse(str1, CultureInfo.InvariantCulture);
			}
			object obj2 = value;
			obj1 = obj2;
			if (obj2 is float)
			{
				return new BigInteger((float)obj1);
			}
			object obj3 = value;
			obj1 = obj3;
			if (obj3 is double)
			{
				return new BigInteger((double)obj1);
			}
			object obj4 = value;
			obj1 = obj4;
			if (obj4 is decimal)
			{
				return new BigInteger((decimal)obj1);
			}
			object obj5 = value;
			obj1 = obj5;
			if (obj5 is int)
			{
				return new BigInteger((int)obj1);
			}
			object obj6 = value;
			obj1 = obj6;
			if (obj6 is long)
			{
				return new BigInteger((long)obj1);
			}
			object obj7 = value;
			obj1 = obj7;
			if (obj7 is uint)
			{
				return new BigInteger((uint)obj1);
			}
			object obj8 = value;
			obj1 = obj8;
			if (obj8 is ulong)
			{
				return new BigInteger((ulong)obj1);
			}
			byte[] numArray = value as byte[];
			byte[] numArray1 = numArray;
			if (numArray == null)
			{
				throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
			}
			return new BigInteger(numArray1);
		}

		private static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object value)
		{
			bool flag;
			try
			{
				if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) != ConvertUtils.ConvertResult.Success)
				{
					value = null;
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			catch
			{
				value = null;
				flag = false;
			}
			return flag;
		}

		public static bool TryConvertGuid(string s, out Guid g)
		{
			return Guid.TryParseExact(s, "D", out g);
		}

		private static ConvertUtils.ConvertResult TryConvertInternal(object initialValue, CultureInfo culture, Type targetType, out object value)
		{
			Version version;
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				value = initialValue;
				return ConvertUtils.ConvertResult.Success;
			}
			if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
			{
				if (targetType.IsEnum())
				{
					if (initialValue is string)
					{
						value = Enum.Parse(targetType, initialValue.ToString(), true);
						return ConvertUtils.ConvertResult.Success;
					}
					if (ConvertUtils.IsInteger(initialValue))
					{
						value = Enum.ToObject(targetType, initialValue);
						return ConvertUtils.ConvertResult.Success;
					}
				}
				value = Convert.ChangeType(initialValue, targetType, culture);
				return ConvertUtils.ConvertResult.Success;
			}
			object obj = initialValue;
			object obj1 = obj;
			if (obj is DateTime)
			{
				DateTime dateTime = (DateTime)obj1;
				if (targetType == typeof(DateTimeOffset))
				{
					value = new DateTimeOffset(dateTime);
					return ConvertUtils.ConvertResult.Success;
				}
			}
			byte[] numArray = initialValue as byte[];
			byte[] numArray1 = numArray;
			if (numArray != null && targetType == typeof(Guid))
			{
				value = new Guid(numArray1);
				return ConvertUtils.ConvertResult.Success;
			}
			object obj2 = initialValue;
			obj1 = obj2;
			if (obj2 is Guid)
			{
				Guid guid = (Guid)obj1;
				if (targetType == typeof(byte[]))
				{
					value = guid.ToByteArray();
					return ConvertUtils.ConvertResult.Success;
				}
			}
			string str = initialValue as string;
			string str1 = str;
			if (str != null)
			{
				if (targetType == typeof(Guid))
				{
					value = new Guid(str1);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(Uri))
				{
					value = new Uri(str1, UriKind.RelativeOrAbsolute);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(TimeSpan))
				{
					value = ConvertUtils.ParseTimeSpan(str1);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(byte[]))
				{
					value = Convert.FromBase64String(str1);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(Version))
				{
					if (Version.TryParse(str1, out version))
					{
						value = version;
						return ConvertUtils.ConvertResult.Success;
					}
					value = null;
					return ConvertUtils.ConvertResult.NoValidConversion;
				}
				if (typeof(Type).IsAssignableFrom(targetType))
				{
					value = Type.GetType(str1, true);
					return ConvertUtils.ConvertResult.Success;
				}
			}
			if (targetType == typeof(BigInteger))
			{
				value = ConvertUtils.ToBigInteger(initialValue);
				return ConvertUtils.ConvertResult.Success;
			}
			object obj3 = initialValue;
			obj1 = obj3;
			if (obj3 is BigInteger)
			{
				value = ConvertUtils.FromBigInteger((BigInteger)obj1, targetType);
				return ConvertUtils.ConvertResult.Success;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				value = converter.ConvertTo(null, culture, initialValue, targetType);
				return ConvertUtils.ConvertResult.Success;
			}
			TypeConverter typeConverter = TypeDescriptor.GetConverter(targetType);
			if (typeConverter != null && typeConverter.CanConvertFrom(type))
			{
				value = typeConverter.ConvertFrom(null, culture, initialValue);
				return ConvertUtils.ConvertResult.Success;
			}
			if (initialValue == DBNull.Value)
			{
				if (!ReflectionUtils.IsNullable(targetType))
				{
					value = null;
					return ConvertUtils.ConvertResult.CannotConvertNull;
				}
				value = ConvertUtils.EnsureTypeAssignable(null, type, targetType);
				return ConvertUtils.ConvertResult.Success;
			}
			if (!targetType.IsInterface() && !targetType.IsGenericTypeDefinition() && !targetType.IsAbstract())
			{
				value = null;
				return ConvertUtils.ConvertResult.NoValidConversion;
			}
			value = null;
			return ConvertUtils.ConvertResult.NotInstantiableType;
		}

		public static bool TryHexTextToInt(char[] text, int start, int end, out int value)
		{
			int num;
			value = 0;
			for (int i = start; i < end; i++)
			{
				char chr = text[i];
				if (chr <= '9' && chr >= '0')
				{
					num = chr - 48;
				}
				else if (chr > 'F' || chr < 'A')
				{
					if (chr > 'f' || chr < 'a')
					{
						value = 0;
						return false;
					}
					num = chr - 87;
				}
				else
				{
					num = chr - 55;
				}
				value = value + (num << ((end - 1 - i) * 4 & 31));
			}
			return true;
		}

		public static bool VersionTryParse(string input, out Version result)
		{
			return Version.TryParse(input, out result);
		}

		internal enum ConvertResult
		{
			Success,
			CannotConvertNull,
			NotInstantiableType,
			NoValidConversion
		}
	}
}