using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	public class JValue : JToken, IEquatable<JValue>, IComparable<JValue>, IConvertible, IFormattable, IComparable
	{
		private JTokenType _valueType;

		private Uri _value;

		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				System.Type type;
				System.Type type1;
				Uri uri = this._value;
				if (uri != null)
				{
					type1 = uri.GetType();
				}
				else
				{
					type1 = null;
				}
				if (value != null)
				{
					type = value.GetType();
				}
				else
				{
					type = null;
				}
				if (type1 != type)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		internal JValue(object value, JTokenType type)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._value = value;
			this._valueType = type;
		}

		public JValue(JValue other)
		{
			Class6.yDnXvgqzyB5jw();
			this(other.Value, other.Type);
		}

		public JValue(long value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Integer);
		}

		public JValue(decimal value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Float);
		}

		public JValue(char value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.String);
		}

		[CLSCompliant(false)]
		public JValue(ulong value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Integer);
		}

		public JValue(double value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Float);
		}

		public JValue(float value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Float);
		}

		public JValue(DateTime value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Date);
		}

		public JValue(DateTimeOffset value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Date);
		}

		public JValue(bool value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Boolean);
		}

		public JValue(string value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.String);
		}

		public JValue(Guid value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.Guid);
		}

		public JValue(Uri value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, (value != null ? JTokenType.Uri : JTokenType.Null));
		}

		public JValue(TimeSpan value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JTokenType.TimeSpan);
		}

		public JValue(object value)
		{
			Class6.yDnXvgqzyB5jw();
			this(value, JValue.GetValueType(null, value));
		}

		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		internal static int Compare(JTokenType valueType, Uri objA, object objB)
		{
			object obj;
			decimal num;
			DateTime dateTime;
			DateTimeOffset dateTimeOffset;
			if (objA == objB)
			{
				return 0;
			}
			if (objB == null)
			{
				return 1;
			}
			if (objA == null)
			{
				return -1;
			}
			switch (valueType)
			{
				case JTokenType.Comment:
				case JTokenType.String:
				case JTokenType.Raw:
				{
					string str = Convert.ToString(objA, CultureInfo.InvariantCulture);
					string str1 = Convert.ToString(objB, CultureInfo.InvariantCulture);
					return string.CompareOrdinal(str, str1);
				}
				case JTokenType.Integer:
				{
					Uri uri = objA;
					obj = uri;
					if (uri is BigInteger)
					{
						return JValue.CompareBigInteger((BigInteger)obj, objB);
					}
					object obj1 = objB;
					obj = obj1;
					if (obj1 is BigInteger)
					{
						return -JValue.CompareBigInteger((BigInteger)obj, objA);
					}
					if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
					{
						num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
						return num.CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
					}
					if (objA is float || objB is float || objA is double || objB is double)
					{
						return JValue.CompareFloat(objA, objB);
					}
					long num1 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
					return num1.CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
				}
				case JTokenType.Float:
				{
					Uri uri1 = objA;
					obj = uri1;
					if (uri1 is BigInteger)
					{
						return JValue.CompareBigInteger((BigInteger)obj, objB);
					}
					object obj2 = objB;
					obj = obj2;
					if (obj2 is BigInteger)
					{
						return -JValue.CompareBigInteger((BigInteger)obj, objA);
					}
					if (!(objA is ulong) && !(objB is ulong) && !(objA is decimal) && !(objB is decimal))
					{
						return JValue.CompareFloat(objA, objB);
					}
					num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
					return num.CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				case JTokenType.Boolean:
				{
					bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
					bool flag1 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
					return flag.CompareTo(flag1);
				}
				case JTokenType.Null:
				case JTokenType.Undefined:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
				}
				case JTokenType.Date:
				{
					Uri uri2 = objA;
					obj = uri2;
					if (!(uri2 is DateTime))
					{
						DateTimeOffset dateTimeOffset1 = (DateTimeOffset)objA;
						object obj3 = objB;
						obj = obj3;
						dateTimeOffset = (!(obj3 is DateTimeOffset) ? new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture)) : (DateTimeOffset)obj);
						return dateTimeOffset1.CompareTo(dateTimeOffset);
					}
					DateTime dateTime1 = (DateTime)obj;
					object obj4 = objB;
					obj = obj4;
					dateTime = (!(obj4 is DateTimeOffset) ? Convert.ToDateTime(objB, CultureInfo.InvariantCulture) : ((DateTimeOffset)obj).DateTime);
					return dateTime1.CompareTo(dateTime);
				}
				case JTokenType.Bytes:
				{
					byte[] numArray = objB as byte[];
					byte[] numArray1 = numArray;
					if (numArray == null)
					{
						throw new ArgumentException("Object must be of type byte[].");
					}
					return MiscellaneousUtils.ByteArrayCompare(objA as byte[], numArray1);
				}
				case JTokenType.Guid:
				{
					if (!(objB is Guid))
					{
						throw new ArgumentException("Object must be of type Guid.");
					}
					return ((Guid)objA).CompareTo((Guid)objB);
				}
				case JTokenType.Uri:
				{
					Uri uri3 = objB as Uri;
					if (uri3 == null)
					{
						throw new ArgumentException("Object must be of type Uri.");
					}
					Uri uri4 = (Uri)objA;
					return Comparer<string>.Default.Compare(uri4.ToString(), uri3.ToString());
				}
				case JTokenType.TimeSpan:
				{
					if (!(objB is TimeSpan))
					{
						throw new ArgumentException("Object must be of type TimeSpan.");
					}
					return ((TimeSpan)objA).CompareTo((TimeSpan)objB);
				}
				default:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
				}
			}
		}

		private static int CompareBigInteger(BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			object obj = i2;
			object obj1 = obj;
			if (obj is decimal)
			{
				decimal num1 = (decimal)obj1;
				decimal zero = decimal.Zero;
				return zero.CompareTo(Math.Abs(num1 - Math.Truncate(num1)));
			}
			if (!(i2 is double) && !(i2 is float))
			{
				return num;
			}
			double num2 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
			double num3 = 0;
			return num3.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
		}

		private static int CompareFloat(object objA, object objB)
		{
			double num = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num1 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(num, num1))
			{
				return 0;
			}
			return num.CompareTo(num1);
		}

		public int CompareTo(JValue obj)
		{
			JTokenType jTokenType;
			if (obj == null)
			{
				return 1;
			}
			if (this._valueType == JTokenType.String)
			{
				if (this._valueType == obj._valueType)
				{
					jTokenType = this._valueType;
					return JValue.Compare(jTokenType, this._value, obj._value);
				}
				jTokenType = obj._valueType;
				return JValue.Compare(jTokenType, this._value, obj._value);
			}
			jTokenType = this._valueType;
			return JValue.Compare(jTokenType, this._value, obj._value);
		}

		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		internal override bool DeepEquals(JToken node)
		{
			JValue jValue = node as JValue;
			JValue jValue1 = jValue;
			if (jValue == null)
			{
				return false;
			}
			if (jValue1 == this)
			{
				return true;
			}
			return JValue.ValuesEquals(this, jValue1);
		}

		public bool Equals(JValue other)
		{
			if (other == null)
			{
				return false;
			}
			return JValue.ValuesEquals(this, other);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as JValue);
		}

		internal override int GetDeepHashCode()
		{
			return this._valueType.GetHashCode() ^ (this._value != null ? this._value.GetHashCode() : 0);
		}

		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValue.Class2());
		}

		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (!current.HasValue)
			{
				return JTokenType.String;
			}
			JTokenType valueOrDefault = current.GetValueOrDefault();
			if (valueOrDefault != JTokenType.Comment && valueOrDefault != JTokenType.String)
			{
				if (valueOrDefault != JTokenType.Raw)
				{
					return JTokenType.String;
				}
			}
			return current.GetValueOrDefault();
		}

		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (!(value is TimeSpan))
			{
				throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
			}
			return JTokenType.TimeSpan;
		}

		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			string str;
			string str1;
			if (objA is string || objB is string)
			{
				if (operation != ExpressionType.Add)
				{
					if (operation != ExpressionType.AddAssign)
					{
						goto Label0;
					}
				}
				if (objA != null)
				{
					str = objA.ToString();
				}
				else
				{
					str = null;
				}
				if (objB != null)
				{
					str1 = objB.ToString();
				}
				else
				{
					str1 = null;
				}
				result = string.Concat(str, str1);
				return true;
			}
			if (objA is BigInteger || objB is BigInteger)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
				BigInteger bigInteger1 = ConvertUtils.ToBigInteger(objB);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation > ExpressionType.Divide)
					{
						if (operation == ExpressionType.Multiply)
						{
							result = bigInteger * bigInteger1;
							return true;
						}
						if (operation == ExpressionType.Subtract)
						{
							result = bigInteger - bigInteger1;
							return true;
						}
						result = null;
						return false;
					}
					else
					{
						if (operation == ExpressionType.Add)
						{
							result = bigInteger + bigInteger1;
							return true;
						}
						if (operation == ExpressionType.Divide)
						{
							result = bigInteger / bigInteger1;
							return true;
						}
						result = null;
						return false;
					}
				}
				else if (operation > ExpressionType.DivideAssign)
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						result = bigInteger * bigInteger1;
						return true;
					}
					if (operation == ExpressionType.SubtractAssign)
					{
						result = bigInteger - bigInteger1;
						return true;
					}
					else
					{
						result = null;
						return false;
					}
				}
				else
				{
					if (operation == ExpressionType.AddAssign)
					{
						result = bigInteger + bigInteger1;
						return true;
					}
					if (operation == ExpressionType.DivideAssign)
					{
						result = bigInteger / bigInteger1;
						return true;
					}
					result = null;
					return false;
				}
				result = bigInteger * bigInteger1;
				return true;
			}
			else if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				decimal num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
				decimal num1 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation > ExpressionType.Divide)
					{
						if (operation == ExpressionType.Multiply)
						{
							result = num * num1;
							return true;
						}
						if (operation == ExpressionType.Subtract)
						{
							result = num - num1;
							return true;
						}
						result = null;
						return false;
					}
					else
					{
						if (operation == ExpressionType.Add)
						{
							result = num + num1;
							return true;
						}
						if (operation == ExpressionType.Divide)
						{
							result = num / num1;
							return true;
						}
						result = null;
						return false;
					}
				}
				else if (operation > ExpressionType.DivideAssign)
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						result = num * num1;
						return true;
					}
					if (operation == ExpressionType.SubtractAssign)
					{
						result = num - num1;
						return true;
					}
					result = null;
					return false;
				}
				else
				{
					if (operation == ExpressionType.AddAssign)
					{
						result = num + num1;
						return true;
					}
					if (operation == ExpressionType.DivideAssign)
					{
						result = num / num1;
						return true;
					}
					result = null;
					return false;
				}
				result = num * num1;
				return true;
			}
			else if (objA is float || objB is float || objA is double || objB is double)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				double num2 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
				double num3 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation > ExpressionType.Divide)
					{
						if (operation == ExpressionType.Multiply)
						{
							result = num2 * num3;
							return true;
						}
						if (operation == ExpressionType.Subtract)
						{
							result = num2 - num3;
							return true;
						}
						result = null;
						return false;
					}
					else
					{
						if (operation == ExpressionType.Add)
						{
							result = num2 + num3;
							return true;
						}
						if (operation == ExpressionType.Divide)
						{
							result = num2 / num3;
							return true;
						}
						result = null;
						return false;
					}
				}
				else if (operation > ExpressionType.DivideAssign)
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						result = num2 * num3;
						return true;
					}
					if (operation == ExpressionType.SubtractAssign)
					{
						result = num2 - num3;
						return true;
					}
					result = null;
					return false;
				}
				else
				{
					if (operation == ExpressionType.AddAssign)
					{
						result = num2 + num3;
						return true;
					}
					if (operation == ExpressionType.DivideAssign)
					{
						result = num2 / num3;
						return true;
					}
					result = null;
					return false;
				}
				result = num2 * num3;
				return true;
			}
			else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				long num4 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
				long num5 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation > ExpressionType.Divide)
					{
						if (operation == ExpressionType.Multiply)
						{
							result = num4 * num5;
							return true;
						}
						if (operation == ExpressionType.Subtract)
						{
							result = num4 - num5;
							return true;
						}
						result = null;
						return false;
					}
					else
					{
						if (operation == ExpressionType.Add)
						{
							result = num4 + num5;
							return true;
						}
						if (operation == ExpressionType.Divide)
						{
							result = num4 / num5;
							return true;
						}
						result = null;
						return false;
					}
				}
				else if (operation > ExpressionType.DivideAssign)
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						result = num4 * num5;
						return true;
					}
					if (operation == ExpressionType.SubtractAssign)
					{
						result = num4 - num5;
						return true;
					}
					result = null;
					return false;
				}
				else
				{
					if (operation == ExpressionType.AddAssign)
					{
						result = num4 + num5;
						return true;
					}
					if (operation == ExpressionType.DivideAssign)
					{
						result = num4 / num5;
						return true;
					}
					result = null;
					return false;
				}
				result = num4 * num5;
				return true;
			}
			result = null;
			return false;
		}

		int System.IComparable.CompareTo(object obj)
		{
			JTokenType jTokenType;
			object value;
			JTokenType jTokenType1;
			if (obj == null)
			{
				return 1;
			}
			JValue jValue = obj as JValue;
			JValue jValue1 = jValue;
			if (jValue == null)
			{
				value = obj;
				jTokenType = this._valueType;
			}
			else
			{
				value = jValue1.Value;
				if (this._valueType == JTokenType.String)
				{
					if (this._valueType == jValue1._valueType)
					{
						goto Label2;
					}
					jTokenType1 = jValue1._valueType;
					goto Label0;
				}
			Label2:
				jTokenType1 = this._valueType;
			Label0:
				jTokenType = jTokenType1;
			}
			return JValue.Compare(jTokenType, this._value, value);
		}

		TypeCode System.IConvertible.GetTypeCode()
		{
			if (this._value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = this._value as IConvertible;
			IConvertible convertible1 = convertible;
			if (convertible == null)
			{
				return TypeCode.Object;
			}
			return convertible1.GetTypeCode();
		}

		bool System.IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)this;
		}

		byte System.IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)this;
		}

		char System.IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)this;
		}

		DateTime System.IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)this;
		}

		decimal System.IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)this;
		}

		double System.IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)((double)this);
		}

		short System.IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)this;
		}

		int System.IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		long System.IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)this;
		}

		sbyte System.IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)this;
		}

		float System.IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)((float)this);
		}

		object System.IConvertible.ToType(System.Type conversionType, IFormatProvider provider)
		{
			return base.ToObject(conversionType);
		}

		ushort System.IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)this;
		}

		uint System.IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)this;
		}

		ulong System.IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)this;
		}

		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			IFormattable formattable1 = formattable;
			if (formattable == null)
			{
				return this._value.ToString();
			}
			return formattable1.ToString(format, formatProvider);
		}

		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			if (v1 == v2)
			{
				return true;
			}
			if (v1._valueType != v2._valueType)
			{
				return false;
			}
			return JValue.Compare(v1._valueType, v1._value, v2._value) == 0;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			object obj;
			string str;
			string str1;
			string str2;
			Guid? nullable;
			TimeSpan? nullable1;
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (this._valueType)
			{
				case JTokenType.Comment:
				{
					JsonWriter jsonWriter = writer;
					Uri uri = this._value;
					if (uri != null)
					{
						str = uri.ToString();
					}
					else
					{
						str = null;
					}
					jsonWriter.WriteComment(str);
					return;
				}
				case JTokenType.Integer:
				{
					Uri uri1 = this._value;
					obj = uri1;
					if (uri1 is int)
					{
						writer.WriteValue((int)obj);
						return;
					}
					Uri uri2 = this._value;
					obj = uri2;
					if (uri2 is long)
					{
						writer.WriteValue((long)obj);
						return;
					}
					Uri uri3 = this._value;
					obj = uri3;
					if (uri3 is ulong)
					{
						writer.WriteValue((ulong)obj);
						return;
					}
					Uri uri4 = this._value;
					obj = uri4;
					if (!(uri4 is BigInteger))
					{
						writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
						return;
					}
					writer.WriteValue((BigInteger)obj);
					return;
				}
				case JTokenType.Float:
				{
					Uri uri5 = this._value;
					obj = uri5;
					if (uri5 is decimal)
					{
						writer.WriteValue((decimal)obj);
						return;
					}
					Uri uri6 = this._value;
					obj = uri6;
					if (uri6 is double)
					{
						writer.WriteValue((double)obj);
						return;
					}
					Uri uri7 = this._value;
					obj = uri7;
					if (uri7 is float)
					{
						writer.WriteValue((float)obj);
						return;
					}
					writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
					return;
				}
				case JTokenType.String:
				{
					JsonWriter jsonWriter1 = writer;
					Uri uri8 = this._value;
					if (uri8 != null)
					{
						str1 = uri8.ToString();
					}
					else
					{
						str1 = null;
					}
					jsonWriter1.WriteValue(str1);
					return;
				}
				case JTokenType.Boolean:
				{
					writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
					return;
				}
				case JTokenType.Null:
				{
					writer.WriteNull();
					return;
				}
				case JTokenType.Undefined:
				{
					writer.WriteUndefined();
					return;
				}
				case JTokenType.Date:
				{
					Uri uri9 = this._value;
					obj = uri9;
					if (uri9 is DateTimeOffset)
					{
						writer.WriteValue((DateTimeOffset)obj);
						return;
					}
					writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
					return;
				}
				case JTokenType.Raw:
				{
					JsonWriter jsonWriter2 = writer;
					Uri uri10 = this._value;
					if (uri10 != null)
					{
						str2 = uri10.ToString();
					}
					else
					{
						str2 = null;
					}
					jsonWriter2.WriteRawValue(str2);
					return;
				}
				case JTokenType.Bytes:
				{
					writer.WriteValue((byte[])this._value);
					return;
				}
				case JTokenType.Guid:
				{
					JsonWriter jsonWriter3 = writer;
					if (this._value != null)
					{
						nullable = (Guid?)this._value;
					}
					else
					{
						nullable = null;
					}
					jsonWriter3.WriteValue(nullable);
					return;
				}
				case JTokenType.Uri:
				{
					writer.WriteValue((Uri)this._value);
					return;
				}
				case JTokenType.TimeSpan:
				{
					JsonWriter jsonWriter4 = writer;
					if (this._value != null)
					{
						nullable1 = (TimeSpan?)this._value;
					}
					else
					{
						nullable1 = null;
					}
					jsonWriter4.WriteValue(nullable1);
					return;
				}
				default:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
				}
			}
		}

		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			object obj;
			string str;
			string str1;
			string str2;
			Guid? nullable;
			TimeSpan? nullable1;
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return AsyncUtils.CompletedTask;
				}
			}
			switch (this._valueType)
			{
				case JTokenType.Comment:
				{
					JsonWriter jsonWriter = writer;
					Uri uri = this._value;
					if (uri != null)
					{
						str = uri.ToString();
					}
					else
					{
						str = null;
					}
					return jsonWriter.WriteCommentAsync(str, cancellationToken);
				}
				case JTokenType.Integer:
				{
					Uri uri1 = this._value;
					obj = uri1;
					if (uri1 is int)
					{
						return writer.WriteValueAsync((int)obj, cancellationToken);
					}
					Uri uri2 = this._value;
					obj = uri2;
					if (uri2 is long)
					{
						return writer.WriteValueAsync((long)obj, cancellationToken);
					}
					Uri uri3 = this._value;
					obj = uri3;
					if (uri3 is ulong)
					{
						return writer.WriteValueAsync((ulong)obj, cancellationToken);
					}
					Uri uri4 = this._value;
					obj = uri4;
					if (!(uri4 is BigInteger))
					{
						return writer.WriteValueAsync(Convert.ToInt64(this._value, CultureInfo.InvariantCulture), cancellationToken);
					}
					return writer.WriteValueAsync((BigInteger)obj, cancellationToken);
				}
				case JTokenType.Float:
				{
					Uri uri5 = this._value;
					obj = uri5;
					if (uri5 is decimal)
					{
						return writer.WriteValueAsync((decimal)obj, cancellationToken);
					}
					Uri uri6 = this._value;
					obj = uri6;
					if (uri6 is double)
					{
						return writer.WriteValueAsync((double)obj, cancellationToken);
					}
					Uri uri7 = this._value;
					obj = uri7;
					if (uri7 is float)
					{
						return writer.WriteValueAsync((float)obj, cancellationToken);
					}
					return writer.WriteValueAsync(Convert.ToDouble(this._value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JTokenType.String:
				{
					JsonWriter jsonWriter1 = writer;
					Uri uri8 = this._value;
					if (uri8 != null)
					{
						str1 = uri8.ToString();
					}
					else
					{
						str1 = null;
					}
					return jsonWriter1.WriteValueAsync(str1, cancellationToken);
				}
				case JTokenType.Boolean:
				{
					return writer.WriteValueAsync(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JTokenType.Null:
				{
					return writer.WriteNullAsync(cancellationToken);
				}
				case JTokenType.Undefined:
				{
					return writer.WriteUndefinedAsync(cancellationToken);
				}
				case JTokenType.Date:
				{
					Uri uri9 = this._value;
					obj = uri9;
					if (uri9 is DateTimeOffset)
					{
						return writer.WriteValueAsync((DateTimeOffset)obj, cancellationToken);
					}
					return writer.WriteValueAsync(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JTokenType.Raw:
				{
					JsonWriter jsonWriter2 = writer;
					Uri uri10 = this._value;
					if (uri10 != null)
					{
						str2 = uri10.ToString();
					}
					else
					{
						str2 = null;
					}
					return jsonWriter2.WriteRawValueAsync(str2, cancellationToken);
				}
				case JTokenType.Bytes:
				{
					return writer.WriteValueAsync((byte[])this._value, cancellationToken);
				}
				case JTokenType.Guid:
				{
					JsonWriter jsonWriter3 = writer;
					if (this._value != null)
					{
						nullable = (Guid?)this._value;
					}
					else
					{
						nullable = null;
					}
					return jsonWriter3.WriteValueAsync(nullable, cancellationToken);
				}
				case JTokenType.Uri:
				{
					return writer.WriteValueAsync((Uri)this._value, cancellationToken);
				}
				case JTokenType.TimeSpan:
				{
					JsonWriter jsonWriter4 = writer;
					if (this._value != null)
					{
						nullable1 = (TimeSpan?)this._value;
					}
					else
					{
						nullable1 = null;
					}
					return jsonWriter4.WriteValueAsync(nullable1, cancellationToken);
				}
				default:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
				}
			}
		}

		private class Class2 : DynamicProxy<JValue>
		{
			public Class2()
			{
				Class6.yDnXvgqzyB5jw();
				base();
			}

			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, out object result)
			{
				JValue jValue = arg as JValue;
				object obj = (jValue != null ? jValue.Value : arg);
				ExpressionType operation = binder.Operation;
				if (operation > ExpressionType.NotEqual)
				{
					if (operation > ExpressionType.AddAssign)
					{
						if (operation == ExpressionType.DivideAssign || operation == ExpressionType.MultiplyAssign || operation == ExpressionType.SubtractAssign)
						{
							if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
							{
								result = new JValue(result);
								return true;
							}
							result = null;
							return false;
						}
						result = null;
						return false;
					}
					else
					{
						if (operation == ExpressionType.Subtract || operation == ExpressionType.AddAssign)
						{
							if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
							{
								result = new JValue(result);
								return true;
							}
							result = null;
							return false;
						}
						result = null;
						return false;
					}
				}
				else if (operation > ExpressionType.LessThanOrEqual)
				{
					if (operation == ExpressionType.Multiply)
					{
						if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
						{
							result = new JValue(result);
							return true;
						}
						result = null;
						return false;
					}
					if (operation == ExpressionType.NotEqual)
					{
						result = JValue.Compare(instance.Type, instance.Value, obj) != 0;
						return true;
					}
					result = null;
					return false;
				}
				else if (operation != ExpressionType.Add)
				{
					switch (operation)
					{
						case ExpressionType.Divide:
						{
							break;
						}
						case ExpressionType.Equal:
						{
							result = JValue.Compare(instance.Type, instance.Value, obj) == 0;
							return true;
						}
						case ExpressionType.ExclusiveOr:
						case ExpressionType.Invoke:
						case ExpressionType.Lambda:
						case ExpressionType.LeftShift:
						{
							result = null;
							return false;
						}
						case ExpressionType.GreaterThan:
						{
							result = JValue.Compare(instance.Type, instance.Value, obj) > 0;
							return true;
						}
						case ExpressionType.GreaterThanOrEqual:
						{
							result = JValue.Compare(instance.Type, instance.Value, obj) >= 0;
							return true;
						}
						case ExpressionType.LessThan:
						{
							result = JValue.Compare(instance.Type, instance.Value, obj) < 0;
							return true;
						}
						case ExpressionType.LessThanOrEqual:
						{
							result = JValue.Compare(instance.Type, instance.Value, obj) <= 0;
							return true;
						}
						default:
						{
							result = null;
							return false;
						}
					}
				}
				if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
				{
					result = new JValue(result);
					return true;
				}
				result = null;
				return false;
			}

			public override bool TryConvert(JValue instance, ConvertBinder binder, out object result)
			{
				if (binder.Type == typeof(JValue) || binder.Type == typeof(JToken))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}
		}
	}
}