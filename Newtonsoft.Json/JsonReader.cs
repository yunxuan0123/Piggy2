using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json
{
	public abstract class JsonReader : IDisposable
	{
		private JsonToken _tokenType;

		private object _value;

		internal char _quoteChar;

		internal JsonReader.State _currentState;

		private JsonPosition _currentPosition;

		private CultureInfo _culture;

		private Newtonsoft.Json.DateTimeZoneHandling _dateTimeZoneHandling;

		private int? _maxDepth;

		private bool _hasExceededMaxDepth;

		internal Newtonsoft.Json.DateParseHandling _dateParseHandling;

		internal Newtonsoft.Json.FloatParseHandling _floatParseHandling;

		private string _dateFormatString;

		private List<JsonPosition> _stack;

		public bool CloseInput
		{
			get;
			set;
		}

		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		public string DateFormatString
		{
			get
			{
				return this._dateFormatString;
			}
			set
			{
				this._dateFormatString = value;
			}
		}

		public Newtonsoft.Json.DateParseHandling DateParseHandling
		{
			get
			{
				return this._dateParseHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.DateParseHandling.None || value > Newtonsoft.Json.DateParseHandling.DateTimeOffset)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateParseHandling = value;
			}
		}

		public Newtonsoft.Json.DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._dateTimeZoneHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.DateTimeZoneHandling.Local || value > Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateTimeZoneHandling = value;
			}
		}

		public virtual int Depth
		{
			get
			{
				int count;
				List<JsonPosition> jsonPositions = this._stack;
				if (jsonPositions != null)
				{
					count = jsonPositions.Count;
				}
				else
				{
					count = 0;
				}
				int num = count;
				if (JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None)
				{
					return num;
				}
				return num + 1;
			}
		}

		public Newtonsoft.Json.FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.FloatParseHandling.Double || value > Newtonsoft.Json.FloatParseHandling.Decimal)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatParseHandling = value;
			}
		}

		public int? MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
			set
			{
				int? nullable = value;
				if (nullable.GetValueOrDefault() <= 0 & nullable.HasValue)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				this._maxDepth = value;
			}
		}

		public virtual string Path
		{
			get
			{
				JsonPosition? nullable;
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				if ((this._currentState == JsonReader.State.ArrayStart || this._currentState == JsonReader.State.ConstructorStart ? false : this._currentState != JsonReader.State.ObjectStart))
				{
					nullable = new JsonPosition?(this._currentPosition);
				}
				else
				{
					nullable = null;
				}
				return JsonPosition.BuildPath(this._stack, nullable);
			}
		}

		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		public bool SupportMultipleContent
		{
			get;
			set;
		}

		public virtual JsonToken TokenType
		{
			get
			{
				return this._tokenType;
			}
		}

		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		public virtual Type ValueType
		{
			get
			{
				object obj = this._value;
				if (obj != null)
				{
					return obj.GetType();
				}
				return null;
			}
		}

		protected JsonReader()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._currentState = JsonReader.State.Start;
			this._dateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;
			this._dateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
			this._floatParseHandling = Newtonsoft.Json.FloatParseHandling.Double;
			this.CloseInput = true;
		}

		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._tokenType = JsonToken.None;
			this._value = null;
		}

		internal JsonReaderException CreateUnexpectedEndException()
		{
			return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed & disposing)
			{
				this.Close();
			}
		}

		private JsonToken GetContentToken()
		{
			while (this.Read())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType == JsonToken.Comment)
				{
					continue;
				}
				return tokenType;
			}
			this.SetToken(JsonToken.None);
			return JsonToken.None;
		}

		internal JsonPosition GetPosition(int depth)
		{
			if (this._stack == null || depth >= this._stack.Count)
			{
				return this._currentPosition;
			}
			return this._stack[depth];
		}

		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
				case JsonToken.EndObject:
				{
					return JsonContainerType.Object;
				}
				case JsonToken.EndArray:
				{
					return JsonContainerType.Array;
				}
				case JsonToken.EndConstructor:
				{
					return JsonContainerType.Constructor;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
				}
			}
		}

		internal bool MoveToContent()
		{
			JsonToken tokenType = this.TokenType;
			while (true)
			{
				if (tokenType != JsonToken.None)
				{
					if (tokenType != JsonToken.Comment)
					{
						return true;
					}
				}
				if (!this.Read())
				{
					break;
				}
				tokenType = this.TokenType;
			}
			return false;
		}

		internal Task<bool> MoveToContentAsync(CancellationToken cancellationToken)
		{
			JsonToken tokenType = this.TokenType;
			if (tokenType != JsonToken.None)
			{
				if (tokenType != JsonToken.Comment)
				{
					return AsyncUtils.True;
				}
			}
			return this.MoveToContentFromNonContentAsync(cancellationToken);
		}

		private async Task<bool> MoveToContentFromNonContentAsync(CancellationToken cancellationToken)
		{
			JsonReader.<MoveToContentFromNonContentAsync>d__14 variable = new JsonReader.<MoveToContentFromNonContentAsync>d__14();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonReader.<MoveToContentFromNonContentAsync>d__14>(ref variable);
			return variable.<>t__builder.Task;
		}

		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		private JsonContainerType Pop()
		{
			JsonPosition jsonPosition;
			if (this._stack == null || this._stack.Count <= 0)
			{
				jsonPosition = this._currentPosition;
				this._currentPosition = new JsonPosition();
			}
			else
			{
				jsonPosition = this._currentPosition;
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			if (this._maxDepth.HasValue)
			{
				int depth = this.Depth;
				int? nullable = this._maxDepth;
				if (depth <= nullable.GetValueOrDefault() & nullable.HasValue)
				{
					this._hasExceededMaxDepth = false;
				}
			}
			return jsonPosition.Type;
		}

		private void Push(JsonContainerType value)
		{
			this.UpdateScopeWithFinishedValue();
			if (this._currentPosition.Type == JsonContainerType.None)
			{
				this._currentPosition = new JsonPosition(value);
				return;
			}
			if (this._stack == null)
			{
				this._stack = new List<JsonPosition>();
			}
			this._stack.Add(this._currentPosition);
			this._currentPosition = new JsonPosition(value);
			if (this._maxDepth.HasValue)
			{
				int? nullable = this._maxDepth;
				if (this.Depth + 1 > nullable.GetValueOrDefault() & nullable.HasValue && !this._hasExceededMaxDepth)
				{
					this._hasExceededMaxDepth = true;
					throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, this._maxDepth));
				}
			}
		}

		public abstract bool Read();

		internal void ReadAndAssert()
		{
			if (!this.Read())
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		internal bool ReadAndMoveToContent()
		{
			if (!this.Read())
			{
				return false;
			}
			return this.MoveToContent();
		}

		internal async Task<bool> ReadAndMoveToContentAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
			bool flag = await configuredTaskAwaitable;
			bool flag1 = flag;
			if (flag)
			{
				configuredTaskAwaitable = this.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
				flag1 = await configuredTaskAwaitable;
			}
			return flag1;
		}

		private bool ReadArrayElementIntoByteArrayReportDone(List<byte> buffer)
		{
			JsonToken tokenType = this.TokenType;
			if (tokenType > JsonToken.Comment)
			{
				if (tokenType == JsonToken.Integer)
				{
					buffer.Add(Convert.ToByte(this.Value, CultureInfo.InvariantCulture));
					return false;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return true;
				}
			}
			else
			{
				if (tokenType == JsonToken.None)
				{
					throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
				}
				if (tokenType == JsonToken.Comment)
				{
					return false;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
		}

		internal byte[] ReadArrayIntoByteArray()
		{
			List<byte> nums = new List<byte>();
			do
			{
				if (this.Read())
				{
					continue;
				}
				this.SetToken(JsonToken.None);
			}
			while (!this.ReadArrayElementIntoByteArrayReportDone(nums));
			byte[] array = nums.ToArray();
			this.SetToken(JsonToken.Bytes, array, false);
			return array;
		}

		internal async Task<byte[]> ReadArrayIntoByteArrayAsync(CancellationToken cancellationToken)
		{
			JsonReader.<ReadArrayIntoByteArrayAsync>d__5 variable = new JsonReader.<ReadArrayIntoByteArrayAsync>d__5();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonReader.<ReadArrayIntoByteArrayAsync>d__5>(ref variable);
			return variable.<>t__builder.Task;
		}

		public virtual bool? ReadAsBoolean()
		{
			bool flag;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
					case JsonToken.Integer:
					case JsonToken.Float:
					{
						object value = this.Value;
						object obj = value;
						flag = (!(value is BigInteger) ? Convert.ToBoolean(this.Value, CultureInfo.InvariantCulture) : (BigInteger)obj != 0L);
						this.SetToken(JsonToken.Boolean, flag, false);
						return new bool?(flag);
					}
					case JsonToken.String:
					{
						return this.ReadBooleanString((string)this.Value);
					}
					case JsonToken.Boolean:
					{
						return new bool?((bool)this.Value);
					}
					case JsonToken.Null:
					case JsonToken.EndArray:
					{
						break;
					}
					case JsonToken.Undefined:
					case JsonToken.EndObject:
					{
						throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
					default:
					{
						throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
				}
			}
			return null;
		}

		public virtual Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<bool?>() ?? Task.FromResult<bool?>(this.ReadAsBoolean());
		}

		public virtual byte[] ReadAsBytes()
		{
			byte[] numArray;
			Guid guid;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken > JsonToken.String)
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					return null;
				}
				if (contentToken == JsonToken.Bytes)
				{
					object value = this.Value;
					object obj = value;
					if (!(value is Guid))
					{
						return (byte[])this.Value;
					}
					byte[] byteArray = ((Guid)obj).ToByteArray();
					this.SetToken(JsonToken.Bytes, byteArray, false);
					return byteArray;
				}
				else
				{
					throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
				}
			}
			else
			{
				switch (contentToken)
				{
					case JsonToken.None:
					{
						break;
					}
					case JsonToken.StartObject:
					{
						this.ReadIntoWrappedTypeObject();
						byte[] numArray1 = this.ReadAsBytes();
						this.ReaderReadAndAssert();
						if (this.TokenType != JsonToken.EndObject)
						{
							throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
						}
						this.SetToken(JsonToken.Bytes, numArray1, false);
						return numArray1;
					}
					case JsonToken.StartArray:
					{
						return this.ReadArrayIntoByteArray();
					}
					default:
					{
						if (contentToken == JsonToken.String)
						{
							string str = (string)this.Value;
							if (str.Length != 0)
							{
								numArray = (!ConvertUtils.TryConvertGuid(str, out guid) ? Convert.FromBase64String(str) : guid.ToByteArray());
							}
							else
							{
								numArray = CollectionUtils.ArrayEmpty<byte>();
							}
							this.SetToken(JsonToken.Bytes, numArray, false);
							return numArray;
						}
						throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
				}
			}
			return null;
		}

		public virtual Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<byte[]>() ?? Task.FromResult<byte[]>(this.ReadAsBytes());
		}

		public virtual DateTime? ReadAsDateTime()
		{
			DateTime? nullable;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken > JsonToken.String)
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					nullable = null;
					return nullable;
				}
				if (contentToken == JsonToken.Date)
				{
					object value = this.Value;
					object obj = value;
					if (value is DateTimeOffset)
					{
						this.SetToken(JsonToken.Date, ((DateTimeOffset)obj).DateTime, false);
					}
					return new DateTime?((DateTime)this.Value);
				}
				else
				{
					throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
				}
			}
			else
			{
				if (contentToken == JsonToken.None)
				{
					nullable = null;
					return nullable;
				}
				if (contentToken == JsonToken.String)
				{
					return this.ReadDateTimeString((string)this.Value);
				}
				throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
			}
			nullable = null;
			return nullable;
		}

		public virtual Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<DateTime?>() ?? Task.FromResult<DateTime?>(this.ReadAsDateTime());
		}

		public virtual DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? nullable;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken > JsonToken.String)
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					nullable = null;
					return nullable;
				}
				if (contentToken == JsonToken.Date)
				{
					object value = this.Value;
					object obj = value;
					if (value is DateTime)
					{
						this.SetToken(JsonToken.Date, new DateTimeOffset((DateTime)obj), false);
					}
					return new DateTimeOffset?((DateTimeOffset)this.Value);
				}
				else
				{
					throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
				}
			}
			else
			{
				if (contentToken == JsonToken.None)
				{
					nullable = null;
					return nullable;
				}
				if (contentToken == JsonToken.String)
				{
					return this.ReadDateTimeOffsetString((string)this.Value);
				}
				throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			nullable = null;
			return nullable;
		}

		public virtual Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<DateTimeOffset?>() ?? Task.FromResult<DateTimeOffset?>(this.ReadAsDateTimeOffset());
		}

		public virtual decimal? ReadAsDecimal()
		{
			decimal num;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
					case JsonToken.Integer:
					case JsonToken.Float:
					{
						object value = this.Value;
						object obj = value;
						object obj1 = obj;
						if (obj is decimal)
						{
							num = (decimal)obj1;
							return new decimal?(num);
						}
						object obj2 = value;
						obj1 = obj2;
						if (!(obj2 is BigInteger))
						{
							try
							{
								num = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								throw JsonReaderException.Create(this, "Could not convert to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, value), exception);
							}
						}
						else
						{
							num = (decimal)((BigInteger)obj1);
						}
						this.SetToken(JsonToken.Float, num, false);
						return new decimal?(num);
					}
					case JsonToken.String:
					{
						return this.ReadDecimalString((string)this.Value);
					}
					case JsonToken.Boolean:
					case JsonToken.Undefined:
					case JsonToken.EndObject:
					{
						throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
					case JsonToken.Null:
					case JsonToken.EndArray:
					{
						break;
					}
					default:
					{
						throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
				}
			}
			return null;
		}

		public virtual Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<decimal?>() ?? Task.FromResult<decimal?>(this.ReadAsDecimal());
		}

		public virtual double? ReadAsDouble()
		{
			double num;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
					case JsonToken.Integer:
					case JsonToken.Float:
					{
						object value = this.Value;
						object obj = value;
						object obj1 = obj;
						if (obj is double)
						{
							num = (double)obj1;
							return new double?(num);
						}
						object obj2 = value;
						obj1 = obj2;
						num = (!(obj2 is BigInteger) ? Convert.ToDouble(value, CultureInfo.InvariantCulture) : (double)((double)((BigInteger)obj1)));
						this.SetToken(JsonToken.Float, num, false);
						return new double?((double)num);
					}
					case JsonToken.String:
					{
						return this.ReadDoubleString((string)this.Value);
					}
					case JsonToken.Boolean:
					case JsonToken.Undefined:
					case JsonToken.EndObject:
					{
						throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
					case JsonToken.Null:
					case JsonToken.EndArray:
					{
						break;
					}
					default:
					{
						throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
				}
			}
			return null;
		}

		public virtual Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = null)
		{
			return Task.FromResult<double?>(this.ReadAsDouble());
		}

		public virtual Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<int?>() ?? Task.FromResult<int?>(this.vmethod_0());
		}

		public virtual string ReadAsString()
		{
			string str;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken == JsonToken.None)
				{
					return null;
				}
				if (contentToken == JsonToken.String)
				{
					return (string)this.Value;
				}
				goto Label0;
			}
			else if (contentToken != JsonToken.Null)
			{
				if (contentToken != JsonToken.EndArray)
				{
					goto Label0;
				}
			}
			return null;
		Label0:
			if (JsonTokenUtils.IsPrimitiveToken(contentToken))
			{
				object value = this.Value;
				if (value != null)
				{
					IFormattable formattable = value as IFormattable;
					IFormattable formattable1 = formattable;
					if (formattable == null)
					{
						Uri uri = value as Uri;
						Uri uri1 = uri;
						str = (uri != null ? uri1.OriginalString : value.ToString());
					}
					else
					{
						str = formattable1.ToString(null, this.Culture);
					}
					this.SetToken(JsonToken.String, str, false);
					return str;
				}
			}
			throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
		}

		public virtual Task<string> ReadAsStringAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<string>() ?? Task.FromResult<string>(this.ReadAsString());
		}

		public virtual Task<bool> ReadAsync(CancellationToken cancellationToken = null)
		{
			return cancellationToken.CancelIfRequestedAsync<bool>() ?? this.Read().ToAsync();
		}

		internal bool? ReadBooleanString(string s)
		{
			bool flag;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (!bool.TryParse(s, out flag))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Boolean, flag, false);
			return new bool?(flag);
		}

		internal DateTimeOffset? ReadDateTimeOffsetString(string s)
		{
			DateTimeOffset dateTimeOffset;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
			{
				this.SetToken(JsonToken.Date, dateTimeOffset, false);
				return new DateTimeOffset?(dateTimeOffset);
			}
			if (!DateTimeOffset.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Date, dateTimeOffset, false);
			return new DateTimeOffset?(dateTimeOffset);
		}

		internal DateTime? ReadDateTimeString(string s)
		{
			DateTime dateTime;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
				this.SetToken(JsonToken.Date, dateTime, false);
				return new DateTime?(dateTime);
			}
			if (!DateTime.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
			this.SetToken(JsonToken.Date, dateTime, false);
			return new DateTime?(dateTime);
		}

		internal decimal? ReadDecimalString(string s)
		{
			decimal num;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (decimal.TryParse(s, NumberStyles.Number, this.Culture, out num))
			{
				this.SetToken(JsonToken.Float, num, false);
				return new decimal?(num);
			}
			if (ConvertUtils.DecimalTryParse(s.ToCharArray(), 0, s.Length, out num) != ParseResult.Success)
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Float, num, false);
			return new decimal?(num);
		}

		internal double? ReadDoubleString(string s)
		{
			double num;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (!double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.Integer | NumberStyles.Float, this.Culture, out num))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Float, num, false);
			return new double?(num);
		}

		internal void ReaderReadAndAssert()
		{
			if (!this.Read())
			{
				throw this.CreateUnexpectedEndException();
			}
		}

		internal async Task ReaderReadAndAssertAsync(CancellationToken cancellationToken)
		{
			if (!await this.ReadAsync(cancellationToken).ConfigureAwait(false))
			{
				throw this.CreateUnexpectedEndException();
			}
		}

		internal bool ReadForType(JsonContract contract, bool hasConverter)
		{
			object underlyingType;
			if (hasConverter)
			{
				return this.Read();
			}
			if (contract != null)
			{
				switch (contract.InternalReadType)
				{
					case ReadType.Read:
					{
						return this.ReadAndMoveToContent();
					}
					case ReadType.const_1:
					{
						this.vmethod_0();
						break;
					}
					case ReadType.const_2:
					{
						bool content = this.ReadAndMoveToContent();
						if (this.TokenType == JsonToken.Undefined)
						{
							CultureInfo invariantCulture = CultureInfo.InvariantCulture;
							if (contract != null)
							{
								underlyingType = contract.UnderlyingType;
								if (underlyingType != null)
								{
									throw JsonReaderException.Create(this, "An undefined token is not a valid {0}.".FormatWith(invariantCulture, underlyingType));
								}
							}
							else
							{
								underlyingType = null;
							}
							underlyingType = typeof(long);
							throw JsonReaderException.Create(this, "An undefined token is not a valid {0}.".FormatWith(invariantCulture, underlyingType));
						}
						return content;
					}
					case ReadType.ReadAsBytes:
					{
						this.ReadAsBytes();
						break;
					}
					case ReadType.ReadAsString:
					{
						this.ReadAsString();
						break;
					}
					case ReadType.ReadAsDecimal:
					{
						this.ReadAsDecimal();
						break;
					}
					case ReadType.ReadAsDateTime:
					{
						this.ReadAsDateTime();
						break;
					}
					case ReadType.ReadAsDateTimeOffset:
					{
						this.ReadAsDateTimeOffset();
						break;
					}
					case ReadType.ReadAsDouble:
					{
						this.ReadAsDouble();
						break;
					}
					case ReadType.ReadAsBoolean:
					{
						this.ReadAsBoolean();
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException();
					}
				}
				return this.TokenType != JsonToken.None;
			}
			return this.ReadAndMoveToContent();
		}

		internal void ReadForTypeAndAssert(JsonContract contract, bool hasConverter)
		{
			if (!this.ReadForType(contract, hasConverter))
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		internal int? ReadInt32String(string s)
		{
			int num;
			if (string.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			if (!int.TryParse(s, NumberStyles.Integer, this.Culture, out num))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Integer, num, false);
			return new int?(num);
		}

		internal void ReadIntoWrappedTypeObject()
		{
			this.ReaderReadAndAssert();
			if (this.Value != null && this.Value.ToString() == "$type")
			{
				this.ReaderReadAndAssert();
				if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
				{
					this.ReaderReadAndAssert();
					if (this.Value.ToString() == "$value")
					{
						return;
					}
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
		}

		private void SetFinished()
		{
			this._currentState = (this.SupportMultipleContent ? JsonReader.State.Start : JsonReader.State.Finished);
		}

		internal void SetPostValueState(bool updateIndex)
		{
			if (this.Peek() != JsonContainerType.None || this.SupportMultipleContent)
			{
				this._currentState = JsonReader.State.PostValue;
			}
			else
			{
				this.SetFinished();
			}
			if (updateIndex)
			{
				this.UpdateScopeWithFinishedValue();
			}
		}

		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
				case JsonContainerType.None:
				{
					this.SetFinished();
					return;
				}
				case JsonContainerType.Object:
				{
					this._currentState = JsonReader.State.Object;
					return;
				}
				case JsonContainerType.Array:
				{
					this._currentState = JsonReader.State.Array;
					return;
				}
				case JsonContainerType.Constructor:
				{
					this._currentState = JsonReader.State.Constructor;
					return;
				}
				default:
				{
					throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
				}
			}
		}

		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null, true);
		}

		protected void SetToken(JsonToken newToken, object value)
		{
			this.SetToken(newToken, value, true);
		}

		protected void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			this._tokenType = newToken;
			this._value = value;
			switch (newToken)
			{
				case JsonToken.StartObject:
				{
					this._currentState = JsonReader.State.ObjectStart;
					this.Push(JsonContainerType.Object);
					return;
				}
				case JsonToken.StartArray:
				{
					this._currentState = JsonReader.State.ArrayStart;
					this.Push(JsonContainerType.Array);
					return;
				}
				case JsonToken.StartConstructor:
				{
					this._currentState = JsonReader.State.ConstructorStart;
					this.Push(JsonContainerType.Constructor);
					return;
				}
				case JsonToken.PropertyName:
				{
					this._currentState = JsonReader.State.Property;
					this._currentPosition.PropertyName = (string)value;
					return;
				}
				case JsonToken.Comment:
				{
					return;
				}
				case JsonToken.Raw:
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					this.SetPostValueState(updateIndex);
					return;
				}
				case JsonToken.EndObject:
				{
					this.ValidateEnd(JsonToken.EndObject);
					return;
				}
				case JsonToken.EndArray:
				{
					this.ValidateEnd(JsonToken.EndArray);
					return;
				}
				case JsonToken.EndConstructor:
				{
					this.ValidateEnd(JsonToken.EndConstructor);
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public void Skip()
		{
			if (this.TokenType == JsonToken.PropertyName)
			{
				this.Read();
			}
			if (JsonTokenUtils.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		public async Task SkipAsync(CancellationToken cancellationToken = null)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
			if (this.TokenType == JsonToken.PropertyName)
			{
				configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			if (JsonTokenUtils.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				do
				{
					configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
				}
				while (await configuredTaskAwaitable && depth < this.Depth);
			}
		}

		void System.IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position++;
			}
		}

		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (this.Peek() == JsonContainerType.None && !this.SupportMultipleContent)
			{
				this.SetFinished();
				return;
			}
			this._currentState = JsonReader.State.PostValue;
		}

		public virtual int? vmethod_0()
		{
			int num;
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
					case JsonToken.Integer:
					case JsonToken.Float:
					{
						object value = this.Value;
						object obj = value;
						object obj1 = obj;
						if (obj is int)
						{
							num = (int)obj1;
							return new int?(num);
						}
						object obj2 = value;
						obj1 = obj2;
						if (!(obj2 is BigInteger))
						{
							try
							{
								num = Convert.ToInt32(value, CultureInfo.InvariantCulture);
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								throw JsonReaderException.Create(this, "Could not convert to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, value), exception);
							}
						}
						else
						{
							num = (int)((BigInteger)obj1);
						}
						this.SetToken(JsonToken.Integer, num, false);
						return new int?(num);
					}
					case JsonToken.String:
					{
						return this.ReadInt32String((string)this.Value);
					}
					case JsonToken.Boolean:
					case JsonToken.Undefined:
					case JsonToken.EndObject:
					{
						throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
					case JsonToken.Null:
					case JsonToken.EndArray:
					{
						break;
					}
					default:
					{
						throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
					}
				}
			}
			return null;
		}

		protected internal enum State
		{
			Start,
			Complete,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			Closed,
			PostValue,
			ConstructorStart,
			Constructor,
			Error,
			Finished
		}
	}
}