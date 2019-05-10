using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json
{
	public abstract class JsonWriter : IDisposable
	{
		private readonly static JsonWriter.State[][] StateArray;

		internal readonly static JsonWriter.State[][] StateArrayTempate;

		private List<JsonPosition> _stack;

		private JsonPosition _currentPosition;

		private JsonWriter.State _currentState;

		private Newtonsoft.Json.Formatting _formatting;

		private Newtonsoft.Json.DateFormatHandling _dateFormatHandling;

		private Newtonsoft.Json.DateTimeZoneHandling _dateTimeZoneHandling;

		private Newtonsoft.Json.StringEscapeHandling _stringEscapeHandling;

		private Newtonsoft.Json.FloatFormatHandling _floatFormatHandling;

		private string _dateFormatString;

		private CultureInfo _culture;

		public bool AutoCompleteOnClose
		{
			get;
			set;
		}

		public bool CloseOutput
		{
			get;
			set;
		}

		internal string ContainerPath
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None || this._stack == null)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(this._stack, null);
			}
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

		public Newtonsoft.Json.DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.DateFormatHandling.IsoDateFormat || value > Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateFormatHandling = value;
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

		public Newtonsoft.Json.FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.FloatFormatHandling.String || value > Newtonsoft.Json.FloatFormatHandling.DefaultValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatFormatHandling = value;
			}
		}

		public Newtonsoft.Json.Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				if (value < Newtonsoft.Json.Formatting.None || value > Newtonsoft.Json.Formatting.Indented)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._formatting = value;
			}
		}

		public string Path
		{
			get
			{
				JsonPosition? nullable;
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				if ((this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.ConstructorStart ? false : this._currentState != JsonWriter.State.ObjectStart))
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

		public Newtonsoft.Json.StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.StringEscapeHandling.Default || value > Newtonsoft.Json.StringEscapeHandling.EscapeHtml)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._stringEscapeHandling = value;
				this.OnStringEscapeHandlingChanged();
			}
		}

		protected internal int Top
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
				if (this.Peek() != JsonContainerType.None)
				{
					num++;
				}
				return num;
			}
		}

		public Newtonsoft.Json.WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
					case JsonWriter.State.Start:
					{
						return Newtonsoft.Json.WriteState.Start;
					}
					case JsonWriter.State.Property:
					{
						return Newtonsoft.Json.WriteState.Property;
					}
					case JsonWriter.State.ObjectStart:
					case JsonWriter.State.Object:
					{
						return Newtonsoft.Json.WriteState.Object;
					}
					case JsonWriter.State.ArrayStart:
					case JsonWriter.State.Array:
					{
						return Newtonsoft.Json.WriteState.Array;
					}
					case JsonWriter.State.ConstructorStart:
					case JsonWriter.State.Constructor:
					{
						return Newtonsoft.Json.WriteState.Constructor;
					}
					case JsonWriter.State.Closed:
					{
						return Newtonsoft.Json.WriteState.Closed;
					}
					case JsonWriter.State.Error:
					{
						return Newtonsoft.Json.WriteState.Error;
					}
					default:
					{
						throw JsonWriterException.Create(this, string.Concat("Invalid state: ", this._currentState), null);
					}
				}
			}
		}

		static JsonWriter()
		{
			Class6.yDnXvgqzyB5jw();
			JsonWriter.StateArrayTempate = new JsonWriter.State[][] { new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("C4C312E2FC5BDFB59C5C048BCB568D6DD6D44220").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("A6DC2102A5AEF8F5B8C5387A080F381336A1853F").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("D68E65A98602F8616EEDC785B546177DF94150BD").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("struct15_0").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("D06BCCE559D1067E5035085507D7504CDC37BF0A").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("F0126FF7D771FAC4CE63479D6D4CF5934A341EC6").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("F0126FF7D771FAC4CE63479D6D4CF5934A341EC6").FieldHandle }, new JsonWriter.State[] { typeof(<PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}).GetField("struct15_1").FieldHandle } };
			JsonWriter.StateArray = JsonWriter.BuildStateArray();
		}

		protected JsonWriter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._currentState = JsonWriter.State.Start;
			this._formatting = Newtonsoft.Json.Formatting.None;
			this._dateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;
			this.CloseOutput = true;
			this.AutoCompleteOnClose = true;
		}

		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			JsonWriter.State stateArray = JsonWriter.StateArray[(int)tokenBeingWritten][(int)this._currentState];
			if (stateArray == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()), null);
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			if (this._formatting == Newtonsoft.Json.Formatting.Indented)
			{
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteIndentSpace();
				}
				if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start)
				{
					this.WriteIndent();
				}
			}
			this._currentState = stateArray;
		}

		private void AutoCompleteAll()
		{
			while (this.Top > 0)
			{
				this.WriteEnd();
			}
		}

		internal Task AutoCompleteAsync(JsonToken tokenBeingWritten, CancellationToken cancellationToken)
		{
			JsonWriter.State state = this._currentState;
			JsonWriter.State stateArray = JsonWriter.StateArray[(int)tokenBeingWritten][(int)state];
			if (stateArray == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), state.ToString()), null);
			}
			this._currentState = stateArray;
			if (this._formatting == Newtonsoft.Json.Formatting.Indented)
			{
				switch (state)
				{
					case JsonWriter.State.Start:
					{
						break;
					}
					case JsonWriter.State.Property:
					{
						return this.WriteIndentSpaceAsync(cancellationToken);
					}
					case JsonWriter.State.ObjectStart:
					{
						if (tokenBeingWritten != JsonToken.PropertyName)
						{
							break;
						}
						return this.WriteIndentAsync(cancellationToken);
					}
					case JsonWriter.State.Object:
					{
						if (tokenBeingWritten == JsonToken.PropertyName)
						{
							return this.AutoCompleteAsync(cancellationToken);
						}
						if (tokenBeingWritten == JsonToken.Comment)
						{
							break;
						}
						return this.WriteValueDelimiterAsync(cancellationToken);
					}
					case JsonWriter.State.ArrayStart:
					case JsonWriter.State.ConstructorStart:
					{
						return this.WriteIndentAsync(cancellationToken);
					}
					case JsonWriter.State.Array:
					case JsonWriter.State.Constructor:
					{
						if (tokenBeingWritten != JsonToken.Comment)
						{
							return this.AutoCompleteAsync(cancellationToken);
						}
						return this.WriteIndentAsync(cancellationToken);
					}
					default:
					{
						if (tokenBeingWritten != JsonToken.PropertyName)
						{
							break;
						}
						return this.WriteIndentAsync(cancellationToken);
					}
				}
			}
			else if (tokenBeingWritten != JsonToken.Comment)
			{
				switch (state)
				{
					case JsonWriter.State.Object:
					case JsonWriter.State.Array:
					case JsonWriter.State.Constructor:
					{
						return this.WriteValueDelimiterAsync(cancellationToken);
					}
				}
			}
			return AsyncUtils.CompletedTask;
		}

		private async Task AutoCompleteAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = this.WriteValueDelimiterAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteIndentAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		private void AutoCompleteClose(JsonContainerType type)
		{
			int complete = this.CalculateLevelsToComplete(type);
			for (int i = 0; i < complete; i++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteNull();
				}
				if (this._formatting == Newtonsoft.Json.Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
				this.UpdateCurrentState();
			}
		}

		internal static JsonWriter.State[][] BuildStateArray()
		{
			List<JsonWriter.State[]> list = JsonWriter.StateArrayTempate.ToList<JsonWriter.State[]>();
			JsonWriter.State[] stateArrayTempate = JsonWriter.StateArrayTempate[0];
			JsonWriter.State[] stateArray = JsonWriter.StateArrayTempate[7];
			ulong[] values = EnumUtils.GetEnumValuesAndNames(typeof(JsonToken)).Values;
			for (int i = 0; i < (int)values.Length; i++)
			{
				ulong num = values[i];
				if (list.Count <= (int)num)
				{
					JsonToken jsonToken = (JsonToken)((int)num);
					if ((int)jsonToken - (int)JsonToken.Integer <= (int)JsonToken.Comment || (int)jsonToken - (int)JsonToken.Date <= (int)JsonToken.StartObject)
					{
						list.Add(stateArray);
					}
					else
					{
						list.Add(stateArrayTempate);
					}
				}
			}
			return list.ToArray();
		}

		private int CalculateLevelsToComplete(JsonContainerType type)
		{
			int num = 0;
			if (this._currentPosition.Type != type)
			{
				int top = this.Top - 2;
				int num1 = top;
				while (num1 >= 0)
				{
					if (this._stack[top - num1].Type != type)
					{
						num1--;
					}
					else
					{
						num = num1 + 2;
						if (num == 0)
						{
							throw JsonWriterException.Create(this, "No token to close.", null);
						}
						return num;
					}
				}
			}
			else
			{
				num = 1;
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			return num;
		}

		private int CalculateWriteTokenFinalDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsEndToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth - 1;
		}

		private int CalculateWriteTokenInitialDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (JsonTokenUtils.IsStartToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth + 1;
		}

		public virtual void Close()
		{
			if (this.AutoCompleteOnClose)
			{
				this.AutoCompleteAll();
			}
		}

		public virtual Task CloseAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.Close();
			return AsyncUtils.CompletedTask;
		}

		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonWriter.State.Closed & disposing)
			{
				this.Close();
			}
		}

		public abstract void Flush();

		public virtual Task FlushAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.Flush();
			return AsyncUtils.CompletedTask;
		}

		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
				case JsonContainerType.Object:
				{
					return JsonToken.EndObject;
				}
				case JsonContainerType.Array:
				{
					return JsonToken.EndArray;
				}
				case JsonContainerType.Constructor:
				{
					return JsonToken.EndConstructor;
				}
				default:
				{
					throw JsonWriterException.Create(this, string.Concat("No close token for type: ", type), null);
				}
			}
		}

		internal void InternalWriteComment()
		{
			this.AutoComplete(JsonToken.Comment);
		}

		internal Task InternalWriteCommentAsync(CancellationToken cancellationToken)
		{
			return this.AutoCompleteAsync(JsonToken.Comment, cancellationToken);
		}

		internal void InternalWriteEnd(JsonContainerType container)
		{
			this.AutoCompleteClose(container);
		}

		internal Task InternalWriteEndAsync(JsonContainerType type, CancellationToken cancellationToken)
		{
			Task task;
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			int complete = this.CalculateLevelsToComplete(type);
			while (true)
			{
				int num = complete;
				complete = num - 1;
				if (num <= 0)
				{
					return AsyncUtils.CompletedTask;
				}
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					task = this.WriteNullAsync(cancellationToken);
					if (!task.IsCompletedSucessfully())
					{
						return this.method_0(task, complete, closeTokenForType, cancellationToken);
					}
				}
				if (this._formatting == Newtonsoft.Json.Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					task = this.WriteIndentAsync(cancellationToken);
					if (!task.IsCompletedSucessfully())
					{
						return this.method_1(task, complete, closeTokenForType, cancellationToken);
					}
				}
				task = this.WriteEndAsync(closeTokenForType, cancellationToken);
				if (!task.IsCompletedSucessfully())
				{
					break;
				}
				this.UpdateCurrentState();
			}
			return this.method_2(task, complete, cancellationToken);
		}

		internal void InternalWritePropertyName(string name)
		{
			this._currentPosition.PropertyName = name;
			this.AutoComplete(JsonToken.PropertyName);
		}

		internal Task InternalWritePropertyNameAsync(string name, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this._currentPosition.PropertyName = name;
			return this.AutoCompleteAsync(JsonToken.PropertyName, cancellationToken);
		}

		internal void InternalWriteRaw()
		{
		}

		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
			this.Push(container);
		}

		internal async Task InternalWriteStartAsync(JsonToken token, JsonContainerType container, CancellationToken cancellationToken)
		{
			this.UpdateScopeWithFinishedValue();
			await this.AutoCompleteAsync(token, cancellationToken).ConfigureAwait(false);
			this.Push(container);
		}

		internal void InternalWriteValue(JsonToken token)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
		}

		internal Task InternalWriteValueAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.UpdateScopeWithFinishedValue();
			return this.AutoCompleteAsync(token, cancellationToken);
		}

		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		private JsonContainerType Pop()
		{
			JsonPosition jsonPosition = this._currentPosition;
			if (this._stack == null || this._stack.Count <= 0)
			{
				this._currentPosition = new JsonPosition();
			}
			else
			{
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			return jsonPosition.Type;
		}

		private void Push(JsonContainerType value)
		{
			if (this._currentPosition.Type != JsonContainerType.None)
			{
				if (this._stack == null)
				{
					this._stack = new List<JsonPosition>();
				}
				this._stack.Add(this._currentPosition);
			}
			this._currentPosition = new JsonPosition(value);
		}

		private static void ResolveConvertibleValue(IConvertible convertible, out PrimitiveTypeCode typeCode, out object value)
		{
			TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
			typeCode = (typeInformation.TypeCode == PrimitiveTypeCode.Object ? PrimitiveTypeCode.String : typeInformation.TypeCode);
			value = convertible.ToType((typeInformation.TypeCode == PrimitiveTypeCode.Object ? typeof(string) : typeInformation.Type), CultureInfo.InvariantCulture);
		}

		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
				case JsonToken.StartObject:
				{
					this.InternalWriteStart(token, JsonContainerType.Object);
					return;
				}
				case JsonToken.StartArray:
				{
					this.InternalWriteStart(token, JsonContainerType.Array);
					return;
				}
				case JsonToken.StartConstructor:
				{
					this.InternalWriteStart(token, JsonContainerType.Constructor);
					return;
				}
				case JsonToken.PropertyName:
				{
					string str = value as string;
					string str1 = str;
					if (str == null)
					{
						throw new ArgumentException("A name is required when setting property name state.", "value");
					}
					this.InternalWritePropertyName(str1);
					return;
				}
				case JsonToken.Comment:
				{
					this.InternalWriteComment();
					return;
				}
				case JsonToken.Raw:
				{
					this.InternalWriteRaw();
					return;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					this.InternalWriteValue(token);
					return;
				}
				case JsonToken.EndObject:
				{
					this.InternalWriteEnd(JsonContainerType.Object);
					return;
				}
				case JsonToken.EndArray:
				{
					this.InternalWriteEnd(JsonContainerType.Array);
					return;
				}
				case JsonToken.EndConstructor:
				{
					this.InternalWriteEnd(JsonContainerType.Constructor);
					return;
				}
				default:
				{
					throw new ArgumentOutOfRangeException("token");
				}
			}
		}

		protected Task SetWriteStateAsync(JsonToken token, object value, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			switch (token)
			{
				case JsonToken.StartObject:
				{
					return this.InternalWriteStartAsync(token, JsonContainerType.Object, cancellationToken);
				}
				case JsonToken.StartArray:
				{
					return this.InternalWriteStartAsync(token, JsonContainerType.Array, cancellationToken);
				}
				case JsonToken.StartConstructor:
				{
					return this.InternalWriteStartAsync(token, JsonContainerType.Constructor, cancellationToken);
				}
				case JsonToken.PropertyName:
				{
					string str = value as string;
					string str1 = str;
					if (str == null)
					{
						throw new ArgumentException("A name is required when setting property name state.", "value");
					}
					return this.InternalWritePropertyNameAsync(str1, cancellationToken);
				}
				case JsonToken.Comment:
				{
					return this.InternalWriteCommentAsync(cancellationToken);
				}
				case JsonToken.Raw:
				{
					return AsyncUtils.CompletedTask;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					return this.InternalWriteValueAsync(token, cancellationToken);
				}
				case JsonToken.EndObject:
				{
					return this.InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);
				}
				case JsonToken.EndArray:
				{
					return this.InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);
				}
				case JsonToken.EndConstructor:
				{
					return this.InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);
				}
				default:
				{
					throw new ArgumentOutOfRangeException("token");
				}
			}
		}

		void System.IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void UpdateCurrentState()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
				case JsonContainerType.None:
				{
					this._currentState = JsonWriter.State.Start;
					return;
				}
				case JsonContainerType.Object:
				{
					this._currentState = JsonWriter.State.Object;
					return;
				}
				case JsonContainerType.Array:
				{
					this._currentState = JsonWriter.State.Array;
					return;
				}
				case JsonContainerType.Constructor:
				{
					this._currentState = JsonWriter.State.Array;
					return;
				}
				default:
				{
					throw JsonWriterException.Create(this, string.Concat("Unknown JsonType: ", jsonContainerType), null);
				}
			}
		}

		internal void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position++;
			}
		}

		public virtual void WriteComment(string text)
		{
			this.InternalWriteComment();
		}

		public virtual Task WriteCommentAsync(string text, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteComment(text);
			return AsyncUtils.CompletedTask;
		}

		private void WriteConstructorDate(JsonReader reader)
		{
			DateTime dateTime;
			string str;
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out str))
			{
				throw JsonWriterException.Create(this, str, null);
			}
			this.WriteValue(dateTime);
		}

		private async Task WriteConstructorDateAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonWriterException.Create(this, string.Concat("Unexpected token when reading date constructor. Expected Integer, got ", reader.TokenType), null);
			}
			DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime((long)reader.Value);
			configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonWriterException.Create(this, string.Concat("Unexpected token when reading date constructor. Expected EndConstructor, got ", reader.TokenType), null);
			}
			await this.WriteValueAsync(dateTime, cancellationToken).ConfigureAwait(false);
		}

		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
				case JsonContainerType.Object:
				{
					this.WriteEndObject();
					return;
				}
				case JsonContainerType.Array:
				{
					this.WriteEndArray();
					return;
				}
				case JsonContainerType.Constructor:
				{
					this.WriteEndConstructor();
					return;
				}
				default:
				{
					throw JsonWriterException.Create(this, string.Concat("Unexpected type when writing end: ", type), null);
				}
			}
		}

		protected virtual void WriteEnd(JsonToken token)
		{
		}

		public virtual void WriteEndArray()
		{
			this.InternalWriteEnd(JsonContainerType.Array);
		}

		public virtual Task WriteEndArrayAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndArray();
			return AsyncUtils.CompletedTask;
		}

		protected virtual Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEnd(token);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteEndAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEnd();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteEndConstructor()
		{
			this.InternalWriteEnd(JsonContainerType.Constructor);
		}

		public virtual Task WriteEndConstructorAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndConstructor();
			return AsyncUtils.CompletedTask;
		}

		internal Task WriteEndInternalAsync(CancellationToken cancellationToken)
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
				case JsonContainerType.Object:
				{
					return this.WriteEndObjectAsync(cancellationToken);
				}
				case JsonContainerType.Array:
				{
					return this.WriteEndArrayAsync(cancellationToken);
				}
				case JsonContainerType.Constructor:
				{
					return this.WriteEndConstructorAsync(cancellationToken);
				}
				default:
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						break;
					}
					else
					{
						return cancellationToken.FromCanceled();
					}
				}
			}
			throw JsonWriterException.Create(this, string.Concat("Unexpected type when writing end: ", jsonContainerType), null);
		}

		public virtual void WriteEndObject()
		{
			this.InternalWriteEnd(JsonContainerType.Object);
		}

		public virtual Task WriteEndObjectAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndObject();
			return AsyncUtils.CompletedTask;
		}

		protected virtual void WriteIndent()
		{
		}

		protected virtual Task WriteIndentAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteIndent();
			return AsyncUtils.CompletedTask;
		}

		protected virtual void WriteIndentSpace()
		{
		}

		protected virtual Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteIndentSpace();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteNull()
		{
			this.InternalWriteValue(JsonToken.Null);
		}

		public virtual Task WriteNullAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteNull();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WritePropertyName(string name)
		{
			this.InternalWritePropertyName(name);
		}

		public virtual void WritePropertyName(string name, bool escape)
		{
			this.WritePropertyName(name);
		}

		public virtual Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WritePropertyName(name);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WritePropertyName(name, escape);
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteRaw(string json)
		{
			this.InternalWriteRaw();
		}

		public virtual Task WriteRawAsync(string json, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteRaw(json);
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteRawValue(string json)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		public virtual Task WriteRawValueAsync(string json, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteRawValue(json);
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteStartArray()
		{
			this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		public virtual Task WriteStartArrayAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartArray();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteStartConstructor(string name)
		{
			this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		public virtual Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartConstructor(name);
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteStartObject()
		{
			this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		public virtual Task WriteStartObjectAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartObject();
			return AsyncUtils.CompletedTask;
		}

		public void WriteToken(JsonReader reader)
		{
			this.WriteToken(reader, true);
		}

		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this.WriteToken(reader, writeChildren, true, true);
		}

		public void WriteToken(JsonToken token, object value)
		{
			object obj;
			string str;
			string str1;
			switch (token)
			{
				case JsonToken.None:
				{
					return;
				}
				case JsonToken.StartObject:
				{
					this.WriteStartObject();
					return;
				}
				case JsonToken.StartArray:
				{
					this.WriteStartArray();
					return;
				}
				case JsonToken.StartConstructor:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					this.WriteStartConstructor(value.ToString());
					return;
				}
				case JsonToken.PropertyName:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					this.WritePropertyName(value.ToString());
					return;
				}
				case JsonToken.Comment:
				{
					if (value != null)
					{
						str = value.ToString();
					}
					else
					{
						str = null;
					}
					this.WriteComment(str);
					return;
				}
				case JsonToken.Raw:
				{
					if (value != null)
					{
						str1 = value.ToString();
					}
					else
					{
						str1 = null;
					}
					this.WriteRawValue(str1);
					return;
				}
				case JsonToken.Integer:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj1 = value;
					obj = obj1;
					if (!(obj1 is BigInteger))
					{
						this.WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
						return;
					}
					this.WriteValue((BigInteger)obj);
					return;
				}
				case JsonToken.Float:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj2 = value;
					obj = obj2;
					if (obj2 is decimal)
					{
						this.WriteValue((decimal)obj);
						return;
					}
					object obj3 = value;
					obj = obj3;
					if (obj3 is double)
					{
						this.WriteValue((double)obj);
						return;
					}
					object obj4 = value;
					obj = obj4;
					if (!(obj4 is float))
					{
						this.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
						return;
					}
					this.WriteValue((float)obj);
					return;
				}
				case JsonToken.String:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					this.WriteValue(value.ToString());
					return;
				}
				case JsonToken.Boolean:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					this.WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
					return;
				}
				case JsonToken.Null:
				{
					this.WriteNull();
					return;
				}
				case JsonToken.Undefined:
				{
					this.WriteUndefined();
					return;
				}
				case JsonToken.EndObject:
				{
					this.WriteEndObject();
					return;
				}
				case JsonToken.EndArray:
				{
					this.WriteEndArray();
					return;
				}
				case JsonToken.EndConstructor:
				{
					this.WriteEndConstructor();
					return;
				}
				case JsonToken.Date:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj5 = value;
					obj = obj5;
					if (!(obj5 is DateTimeOffset))
					{
						this.WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
						return;
					}
					this.WriteValue((DateTimeOffset)obj);
					return;
				}
				case JsonToken.Bytes:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj6 = value;
					obj = obj6;
					if (!(obj6 is Guid))
					{
						this.WriteValue((byte[])value);
						return;
					}
					this.WriteValue((Guid)obj);
					return;
				}
				default:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
				}
			}
		}

		public void WriteToken(JsonToken token)
		{
			this.WriteToken(token, null);
		}

		internal virtual void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			int num = this.CalculateWriteTokenInitialDepth(reader);
			while (true)
			{
				if (writeDateConstructorAsDate)
				{
					if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
					{
						goto Label1;
					}
					this.WriteConstructorDate(reader);
					goto Label0;
				}
			Label1:
				if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					this.WriteToken(reader.TokenType, reader.Value);
				}
			Label0:
				int num1 = num - 1;
				if (!(num1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) & writeChildren))
				{
					break;
				}
				if (!reader.Read())
				{
					break;
				}
			}
			if (num < this.CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		public Task WriteTokenAsync(JsonReader reader, CancellationToken cancellationToken = null)
		{
			return this.WriteTokenAsync(reader, true, cancellationToken);
		}

		public Task WriteTokenAsync(JsonReader reader, bool writeChildren, CancellationToken cancellationToken = null)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			return this.WriteTokenAsync(reader, writeChildren, true, true, cancellationToken);
		}

		public Task WriteTokenAsync(JsonToken token, CancellationToken cancellationToken = null)
		{
			return this.WriteTokenAsync(token, null, cancellationToken);
		}

		public Task WriteTokenAsync(JsonToken token, object value, CancellationToken cancellationToken = null)
		{
			object obj;
			string str;
			string str1;
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			switch (token)
			{
				case JsonToken.None:
				{
					return AsyncUtils.CompletedTask;
				}
				case JsonToken.StartObject:
				{
					return this.WriteStartObjectAsync(cancellationToken);
				}
				case JsonToken.StartArray:
				{
					return this.WriteStartArrayAsync(cancellationToken);
				}
				case JsonToken.StartConstructor:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					return this.WriteStartConstructorAsync(value.ToString(), cancellationToken);
				}
				case JsonToken.PropertyName:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					return this.WritePropertyNameAsync(value.ToString(), cancellationToken);
				}
				case JsonToken.Comment:
				{
					if (value != null)
					{
						str = value.ToString();
					}
					else
					{
						str = null;
					}
					return this.WriteCommentAsync(str, cancellationToken);
				}
				case JsonToken.Raw:
				{
					if (value != null)
					{
						str1 = value.ToString();
					}
					else
					{
						str1 = null;
					}
					return this.WriteRawValueAsync(str1, cancellationToken);
				}
				case JsonToken.Integer:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj1 = value;
					obj = obj1;
					if (!(obj1 is BigInteger))
					{
						return this.WriteValueAsync(Convert.ToInt64(value, CultureInfo.InvariantCulture), cancellationToken);
					}
					return this.WriteValueAsync((BigInteger)obj, cancellationToken);
				}
				case JsonToken.Float:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj2 = value;
					obj = obj2;
					if (obj2 is decimal)
					{
						return this.WriteValueAsync((decimal)obj, cancellationToken);
					}
					object obj3 = value;
					obj = obj3;
					if (obj3 is double)
					{
						return this.WriteValueAsync((double)obj, cancellationToken);
					}
					object obj4 = value;
					obj = obj4;
					if (!(obj4 is float))
					{
						return this.WriteValueAsync(Convert.ToDouble(value, CultureInfo.InvariantCulture), cancellationToken);
					}
					return this.WriteValueAsync((float)obj, cancellationToken);
				}
				case JsonToken.String:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					return this.WriteValueAsync(value.ToString(), cancellationToken);
				}
				case JsonToken.Boolean:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					return this.WriteValueAsync(Convert.ToBoolean(value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JsonToken.Null:
				{
					return this.WriteNullAsync(cancellationToken);
				}
				case JsonToken.Undefined:
				{
					return this.WriteUndefinedAsync(cancellationToken);
				}
				case JsonToken.EndObject:
				{
					return this.WriteEndObjectAsync(cancellationToken);
				}
				case JsonToken.EndArray:
				{
					return this.WriteEndArrayAsync(cancellationToken);
				}
				case JsonToken.EndConstructor:
				{
					return this.WriteEndConstructorAsync(cancellationToken);
				}
				case JsonToken.Date:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj5 = value;
					obj = obj5;
					if (!(obj5 is DateTimeOffset))
					{
						return this.WriteValueAsync(Convert.ToDateTime(value, CultureInfo.InvariantCulture), cancellationToken);
					}
					return this.WriteValueAsync((DateTimeOffset)obj, cancellationToken);
				}
				case JsonToken.Bytes:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj6 = value;
					obj = obj6;
					if (!(obj6 is Guid))
					{
						return this.WriteValueAsync((byte[])value, cancellationToken);
					}
					return this.WriteValueAsync((Guid)obj, cancellationToken);
				}
				default:
				{
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
				}
			}
		}

		internal virtual async Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable;
			bool flag;
			int num;
			int num1 = this.CalculateWriteTokenInitialDepth(reader);
			do
			{
				if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					configuredTaskAwaitable = this.WriteConstructorDateAsync(reader, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
				}
				else if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					configuredTaskAwaitable = this.WriteTokenAsync(reader.TokenType, reader.Value, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
				}
				int num2 = num1 - 1;
				int depth = reader.Depth;
				num = (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0);
				bool flag1 = num2 < depth - num & writeChildren;
				flag = flag1;
				if (!flag1)
				{
					continue;
				}
				ConfiguredTaskAwaitable<bool> configuredTaskAwaitable1 = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				flag = await configuredTaskAwaitable1;
			}
			while (flag);
			if (num1 < this.CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		internal async Task WriteTokenSyncReadingAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			bool flag;
			int num;
			int num1 = this.CalculateWriteTokenInitialDepth(reader);
			do
			{
				if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					this.WriteToken(reader.TokenType, reader.Value);
				}
				else
				{
					this.WriteConstructorDate(reader);
				}
				int num2 = num1 - 1;
				int depth = reader.Depth;
				num = (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0);
				bool flag1 = num2 < depth - num;
				flag = flag1;
				if (!flag1)
				{
					continue;
				}
				ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				flag = await configuredTaskAwaitable;
			}
			while (flag);
			if (num1 < this.CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		public virtual void WriteUndefined()
		{
			this.InternalWriteValue(JsonToken.Undefined);
		}

		public virtual Task WriteUndefinedAsync(CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteUndefined();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteValue(string value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(long value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(float value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(double value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(bool value)
		{
			this.InternalWriteValue(JsonToken.Boolean);
		}

		public virtual void WriteValue(short value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(char value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(byte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(decimal value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(DateTime value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(DateTimeOffset value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(Guid value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(TimeSpan value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(long? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(bool? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(short? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(char? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(byte? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(decimal? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(DateTime? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(Guid? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(TimeSpan? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.Bytes);
		}

		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is BigInteger)
			{
				throw JsonWriter.CreateUnsupportedTypeException(this, value);
			}
			JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			char? nullable;
			bool? nullable1;
			sbyte? nullable2;
			short? nullable3;
			ushort? nullable4;
			int? nullable5;
			byte? nullable6;
			uint? nullable7;
			long? nullable8;
			ulong? nullable9;
			float? nullable10;
			double? nullable11;
			DateTime? nullable12;
			DateTimeOffset? nullable13;
			decimal? nullable14;
			Guid? nullable15;
			TimeSpan? nullable16;
			BigInteger? nullable17;
			while (true)
			{
				switch (typeCode)
				{
					case PrimitiveTypeCode.Char:
					{
						writer.WriteValue((char)value);
						return;
					}
					case PrimitiveTypeCode.CharNullable:
					{
						JsonWriter jsonWriter = writer;
						if (value == null)
						{
							nullable = null;
						}
						else
						{
							nullable = new char?((char)value);
						}
						jsonWriter.WriteValue(nullable);
						return;
					}
					case PrimitiveTypeCode.Boolean:
					{
						writer.WriteValue((bool)value);
						return;
					}
					case PrimitiveTypeCode.BooleanNullable:
					{
						JsonWriter jsonWriter1 = writer;
						if (value == null)
						{
							nullable1 = null;
						}
						else
						{
							nullable1 = new bool?((bool)value);
						}
						jsonWriter1.WriteValue(nullable1);
						return;
					}
					case PrimitiveTypeCode.SByte:
					{
						writer.WriteValue((sbyte)value);
						return;
					}
					case PrimitiveTypeCode.SByteNullable:
					{
						JsonWriter jsonWriter2 = writer;
						if (value == null)
						{
							nullable2 = null;
						}
						else
						{
							nullable2 = new sbyte?((sbyte)value);
						}
						jsonWriter2.WriteValue(nullable2);
						return;
					}
					case PrimitiveTypeCode.Int16:
					{
						writer.WriteValue((short)value);
						return;
					}
					case PrimitiveTypeCode.Int16Nullable:
					{
						JsonWriter jsonWriter3 = writer;
						if (value == null)
						{
							nullable3 = null;
						}
						else
						{
							nullable3 = new short?((short)value);
						}
						jsonWriter3.WriteValue(nullable3);
						return;
					}
					case PrimitiveTypeCode.UInt16:
					{
						writer.WriteValue((ushort)value);
						return;
					}
					case PrimitiveTypeCode.UInt16Nullable:
					{
						JsonWriter jsonWriter4 = writer;
						if (value == null)
						{
							nullable4 = null;
						}
						else
						{
							nullable4 = new ushort?((ushort)value);
						}
						jsonWriter4.WriteValue(nullable4);
						return;
					}
					case PrimitiveTypeCode.Int32:
					{
						writer.WriteValue((int)value);
						return;
					}
					case PrimitiveTypeCode.Int32Nullable:
					{
						JsonWriter jsonWriter5 = writer;
						if (value == null)
						{
							nullable5 = null;
						}
						else
						{
							nullable5 = new int?((int)value);
						}
						jsonWriter5.WriteValue(nullable5);
						return;
					}
					case PrimitiveTypeCode.Byte:
					{
						writer.WriteValue((byte)value);
						return;
					}
					case PrimitiveTypeCode.ByteNullable:
					{
						JsonWriter jsonWriter6 = writer;
						if (value == null)
						{
							nullable6 = null;
						}
						else
						{
							nullable6 = new byte?((byte)value);
						}
						jsonWriter6.WriteValue(nullable6);
						return;
					}
					case PrimitiveTypeCode.UInt32:
					{
						writer.WriteValue((uint)value);
						return;
					}
					case PrimitiveTypeCode.UInt32Nullable:
					{
						JsonWriter jsonWriter7 = writer;
						if (value == null)
						{
							nullable7 = null;
						}
						else
						{
							nullable7 = new uint?((uint)value);
						}
						jsonWriter7.WriteValue(nullable7);
						return;
					}
					case PrimitiveTypeCode.Int64:
					{
						writer.WriteValue((long)value);
						return;
					}
					case PrimitiveTypeCode.Int64Nullable:
					{
						JsonWriter jsonWriter8 = writer;
						if (value == null)
						{
							nullable8 = null;
						}
						else
						{
							nullable8 = new long?((long)value);
						}
						jsonWriter8.WriteValue(nullable8);
						return;
					}
					case PrimitiveTypeCode.UInt64:
					{
						writer.WriteValue((ulong)value);
						return;
					}
					case PrimitiveTypeCode.UInt64Nullable:
					{
						JsonWriter jsonWriter9 = writer;
						if (value == null)
						{
							nullable9 = null;
						}
						else
						{
							nullable9 = new ulong?((ulong)value);
						}
						jsonWriter9.WriteValue(nullable9);
						return;
					}
					case PrimitiveTypeCode.Single:
					{
						writer.WriteValue((float)value);
						return;
					}
					case PrimitiveTypeCode.SingleNullable:
					{
						JsonWriter jsonWriter10 = writer;
						if (value == null)
						{
							nullable10 = null;
						}
						else
						{
							nullable10 = new float?((float)value);
						}
						jsonWriter10.WriteValue(nullable10);
						return;
					}
					case PrimitiveTypeCode.Double:
					{
						writer.WriteValue((double)value);
						return;
					}
					case PrimitiveTypeCode.DoubleNullable:
					{
						JsonWriter jsonWriter11 = writer;
						if (value == null)
						{
							nullable11 = null;
						}
						else
						{
							nullable11 = new double?((double)value);
						}
						jsonWriter11.WriteValue(nullable11);
						return;
					}
					case PrimitiveTypeCode.DateTime:
					{
						writer.WriteValue((DateTime)value);
						return;
					}
					case PrimitiveTypeCode.DateTimeNullable:
					{
						JsonWriter jsonWriter12 = writer;
						if (value == null)
						{
							nullable12 = null;
						}
						else
						{
							nullable12 = new DateTime?((DateTime)value);
						}
						jsonWriter12.WriteValue(nullable12);
						return;
					}
					case PrimitiveTypeCode.DateTimeOffset:
					{
						writer.WriteValue((DateTimeOffset)value);
						return;
					}
					case PrimitiveTypeCode.DateTimeOffsetNullable:
					{
						JsonWriter jsonWriter13 = writer;
						if (value == null)
						{
							nullable13 = null;
						}
						else
						{
							nullable13 = new DateTimeOffset?((DateTimeOffset)value);
						}
						jsonWriter13.WriteValue(nullable13);
						return;
					}
					case PrimitiveTypeCode.Decimal:
					{
						writer.WriteValue((decimal)value);
						return;
					}
					case PrimitiveTypeCode.DecimalNullable:
					{
						JsonWriter jsonWriter14 = writer;
						if (value == null)
						{
							nullable14 = null;
						}
						else
						{
							nullable14 = new decimal?((decimal)value);
						}
						jsonWriter14.WriteValue(nullable14);
						return;
					}
					case PrimitiveTypeCode.Guid:
					{
						writer.WriteValue((Guid)value);
						return;
					}
					case PrimitiveTypeCode.GuidNullable:
					{
						JsonWriter jsonWriter15 = writer;
						if (value == null)
						{
							nullable15 = null;
						}
						else
						{
							nullable15 = new Guid?((Guid)value);
						}
						jsonWriter15.WriteValue(nullable15);
						return;
					}
					case PrimitiveTypeCode.TimeSpan:
					{
						writer.WriteValue((TimeSpan)value);
						return;
					}
					case PrimitiveTypeCode.TimeSpanNullable:
					{
						JsonWriter jsonWriter16 = writer;
						if (value == null)
						{
							nullable16 = null;
						}
						else
						{
							nullable16 = new TimeSpan?((TimeSpan)value);
						}
						jsonWriter16.WriteValue(nullable16);
						return;
					}
					case PrimitiveTypeCode.BigInteger:
					{
						writer.WriteValue((BigInteger)value);
						return;
					}
					case PrimitiveTypeCode.BigIntegerNullable:
					{
						JsonWriter jsonWriter17 = writer;
						if (value == null)
						{
							nullable17 = null;
						}
						else
						{
							nullable17 = new BigInteger?((BigInteger)value);
						}
						jsonWriter17.WriteValue(nullable17);
						return;
					}
					case PrimitiveTypeCode.Uri:
					{
						writer.WriteValue((Uri)value);
						return;
					}
					case PrimitiveTypeCode.String:
					{
						writer.WriteValue((string)value);
						return;
					}
					case PrimitiveTypeCode.Bytes:
					{
						writer.WriteValue((byte[])value);
						return;
					}
					case PrimitiveTypeCode.DBNull:
					{
						break;
					}
					default:
					{
						IConvertible convertible = value as IConvertible;
						IConvertible convertible1 = convertible;
						if (convertible == null)
						{
							if (value != null)
							{
								throw JsonWriter.CreateUnsupportedTypeException(writer, value);
							}
							writer.WriteNull();
							return;
						}
						JsonWriter.ResolveConvertibleValue(convertible1, out typeCode, out value);
						continue;
					}
				}
			}
			writer.WriteNull();
		}

		public virtual Task WriteValueAsync(bool value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(bool? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(char value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(char? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(decimal value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(double value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(double? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(float value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(float? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Guid value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(int value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(int? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(long value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(long? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(object value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(short value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(short? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(string value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Uri value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		internal static Task WriteValueAsync(JsonWriter writer, PrimitiveTypeCode typeCode, object value, CancellationToken cancellationToken)
		{
			char? nullable;
			bool? nullable1;
			sbyte? nullable2;
			short? nullable3;
			ushort? nullable4;
			int? nullable5;
			byte? nullable6;
			uint? nullable7;
			long? nullable8;
			ulong? nullable9;
			float? nullable10;
			double? nullable11;
			DateTime? nullable12;
			DateTimeOffset? nullable13;
			decimal? nullable14;
			Guid? nullable15;
			TimeSpan? nullable16;
			BigInteger? nullable17;
			while (true)
			{
				switch (typeCode)
				{
					case PrimitiveTypeCode.Char:
					{
						return writer.WriteValueAsync((char)value, cancellationToken);
					}
					case PrimitiveTypeCode.CharNullable:
					{
						JsonWriter jsonWriter = writer;
						if (value == null)
						{
							nullable = null;
						}
						else
						{
							nullable = new char?((char)value);
						}
						return jsonWriter.WriteValueAsync(nullable, cancellationToken);
					}
					case PrimitiveTypeCode.Boolean:
					{
						return writer.WriteValueAsync((bool)value, cancellationToken);
					}
					case PrimitiveTypeCode.BooleanNullable:
					{
						JsonWriter jsonWriter1 = writer;
						if (value == null)
						{
							nullable1 = null;
						}
						else
						{
							nullable1 = new bool?((bool)value);
						}
						return jsonWriter1.WriteValueAsync(nullable1, cancellationToken);
					}
					case PrimitiveTypeCode.SByte:
					{
						return writer.WriteValueAsync((sbyte)value, cancellationToken);
					}
					case PrimitiveTypeCode.SByteNullable:
					{
						JsonWriter jsonWriter2 = writer;
						if (value == null)
						{
							nullable2 = null;
						}
						else
						{
							nullable2 = new sbyte?((sbyte)value);
						}
						return jsonWriter2.WriteValueAsync(nullable2, cancellationToken);
					}
					case PrimitiveTypeCode.Int16:
					{
						return writer.WriteValueAsync((short)value, cancellationToken);
					}
					case PrimitiveTypeCode.Int16Nullable:
					{
						JsonWriter jsonWriter3 = writer;
						if (value == null)
						{
							nullable3 = null;
						}
						else
						{
							nullable3 = new short?((short)value);
						}
						return jsonWriter3.WriteValueAsync(nullable3, cancellationToken);
					}
					case PrimitiveTypeCode.UInt16:
					{
						return writer.WriteValueAsync((ushort)value, cancellationToken);
					}
					case PrimitiveTypeCode.UInt16Nullable:
					{
						JsonWriter jsonWriter4 = writer;
						if (value == null)
						{
							nullable4 = null;
						}
						else
						{
							nullable4 = new ushort?((ushort)value);
						}
						return jsonWriter4.WriteValueAsync(nullable4, cancellationToken);
					}
					case PrimitiveTypeCode.Int32:
					{
						return writer.WriteValueAsync((int)value, cancellationToken);
					}
					case PrimitiveTypeCode.Int32Nullable:
					{
						JsonWriter jsonWriter5 = writer;
						if (value == null)
						{
							nullable5 = null;
						}
						else
						{
							nullable5 = new int?((int)value);
						}
						return jsonWriter5.WriteValueAsync(nullable5, cancellationToken);
					}
					case PrimitiveTypeCode.Byte:
					{
						return writer.WriteValueAsync((byte)value, cancellationToken);
					}
					case PrimitiveTypeCode.ByteNullable:
					{
						JsonWriter jsonWriter6 = writer;
						if (value == null)
						{
							nullable6 = null;
						}
						else
						{
							nullable6 = new byte?((byte)value);
						}
						return jsonWriter6.WriteValueAsync(nullable6, cancellationToken);
					}
					case PrimitiveTypeCode.UInt32:
					{
						return writer.WriteValueAsync((uint)value, cancellationToken);
					}
					case PrimitiveTypeCode.UInt32Nullable:
					{
						JsonWriter jsonWriter7 = writer;
						if (value == null)
						{
							nullable7 = null;
						}
						else
						{
							nullable7 = new uint?((uint)value);
						}
						return jsonWriter7.WriteValueAsync(nullable7, cancellationToken);
					}
					case PrimitiveTypeCode.Int64:
					{
						return writer.WriteValueAsync((long)value, cancellationToken);
					}
					case PrimitiveTypeCode.Int64Nullable:
					{
						JsonWriter jsonWriter8 = writer;
						if (value == null)
						{
							nullable8 = null;
						}
						else
						{
							nullable8 = new long?((long)value);
						}
						return jsonWriter8.WriteValueAsync(nullable8, cancellationToken);
					}
					case PrimitiveTypeCode.UInt64:
					{
						return writer.WriteValueAsync((ulong)value, cancellationToken);
					}
					case PrimitiveTypeCode.UInt64Nullable:
					{
						JsonWriter jsonWriter9 = writer;
						if (value == null)
						{
							nullable9 = null;
						}
						else
						{
							nullable9 = new ulong?((ulong)value);
						}
						return jsonWriter9.WriteValueAsync(nullable9, cancellationToken);
					}
					case PrimitiveTypeCode.Single:
					{
						return writer.WriteValueAsync((float)value, cancellationToken);
					}
					case PrimitiveTypeCode.SingleNullable:
					{
						JsonWriter jsonWriter10 = writer;
						if (value == null)
						{
							nullable10 = null;
						}
						else
						{
							nullable10 = new float?((float)value);
						}
						return jsonWriter10.WriteValueAsync(nullable10, cancellationToken);
					}
					case PrimitiveTypeCode.Double:
					{
						return writer.WriteValueAsync((double)value, cancellationToken);
					}
					case PrimitiveTypeCode.DoubleNullable:
					{
						JsonWriter jsonWriter11 = writer;
						if (value == null)
						{
							nullable11 = null;
						}
						else
						{
							nullable11 = new double?((double)value);
						}
						return jsonWriter11.WriteValueAsync(nullable11, cancellationToken);
					}
					case PrimitiveTypeCode.DateTime:
					{
						return writer.WriteValueAsync((DateTime)value, cancellationToken);
					}
					case PrimitiveTypeCode.DateTimeNullable:
					{
						JsonWriter jsonWriter12 = writer;
						if (value == null)
						{
							nullable12 = null;
						}
						else
						{
							nullable12 = new DateTime?((DateTime)value);
						}
						return jsonWriter12.WriteValueAsync(nullable12, cancellationToken);
					}
					case PrimitiveTypeCode.DateTimeOffset:
					{
						return writer.WriteValueAsync((DateTimeOffset)value, cancellationToken);
					}
					case PrimitiveTypeCode.DateTimeOffsetNullable:
					{
						JsonWriter jsonWriter13 = writer;
						if (value == null)
						{
							nullable13 = null;
						}
						else
						{
							nullable13 = new DateTimeOffset?((DateTimeOffset)value);
						}
						return jsonWriter13.WriteValueAsync(nullable13, cancellationToken);
					}
					case PrimitiveTypeCode.Decimal:
					{
						return writer.WriteValueAsync((decimal)value, cancellationToken);
					}
					case PrimitiveTypeCode.DecimalNullable:
					{
						JsonWriter jsonWriter14 = writer;
						if (value == null)
						{
							nullable14 = null;
						}
						else
						{
							nullable14 = new decimal?((decimal)value);
						}
						return jsonWriter14.WriteValueAsync(nullable14, cancellationToken);
					}
					case PrimitiveTypeCode.Guid:
					{
						return writer.WriteValueAsync((Guid)value, cancellationToken);
					}
					case PrimitiveTypeCode.GuidNullable:
					{
						JsonWriter jsonWriter15 = writer;
						if (value == null)
						{
							nullable15 = null;
						}
						else
						{
							nullable15 = new Guid?((Guid)value);
						}
						return jsonWriter15.WriteValueAsync(nullable15, cancellationToken);
					}
					case PrimitiveTypeCode.TimeSpan:
					{
						return writer.WriteValueAsync((TimeSpan)value, cancellationToken);
					}
					case PrimitiveTypeCode.TimeSpanNullable:
					{
						JsonWriter jsonWriter16 = writer;
						if (value == null)
						{
							nullable16 = null;
						}
						else
						{
							nullable16 = new TimeSpan?((TimeSpan)value);
						}
						return jsonWriter16.WriteValueAsync(nullable16, cancellationToken);
					}
					case PrimitiveTypeCode.BigInteger:
					{
						return writer.WriteValueAsync((BigInteger)value, cancellationToken);
					}
					case PrimitiveTypeCode.BigIntegerNullable:
					{
						JsonWriter jsonWriter17 = writer;
						if (value == null)
						{
							nullable17 = null;
						}
						else
						{
							nullable17 = new BigInteger?((BigInteger)value);
						}
						return jsonWriter17.WriteValueAsync(nullable17, cancellationToken);
					}
					case PrimitiveTypeCode.Uri:
					{
						return writer.WriteValueAsync((Uri)value, cancellationToken);
					}
					case PrimitiveTypeCode.String:
					{
						return writer.WriteValueAsync((string)value, cancellationToken);
					}
					case PrimitiveTypeCode.Bytes:
					{
						return writer.WriteValueAsync((byte[])value, cancellationToken);
					}
					case PrimitiveTypeCode.DBNull:
					{
						break;
					}
					default:
					{
						IConvertible convertible = value as IConvertible;
						IConvertible convertible1 = convertible;
						if (convertible == null)
						{
							if (value != null)
							{
								throw JsonWriter.CreateUnsupportedTypeException(writer, value);
							}
							return writer.WriteNullAsync(cancellationToken);
						}
						JsonWriter.ResolveConvertibleValue(convertible1, out typeCode, out value);
						continue;
					}
				}
			}
			return writer.WriteNullAsync(cancellationToken);
		}

		protected virtual void WriteValueDelimiter()
		{
		}

		protected virtual Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValueDelimiter();
			return AsyncUtils.CompletedTask;
		}

		public virtual void WriteWhitespace(string ws)
		{
			this.InternalWriteWhitespace(ws);
		}

		public virtual Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteWhitespace(ws);
			return AsyncUtils.CompletedTask;
		}

		internal enum State
		{
			Start,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			ConstructorStart,
			Constructor,
			Closed,
			Error
		}
	}
}