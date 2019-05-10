using Newtonsoft.Json.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json
{
	public class JsonTextWriter : JsonWriter
	{
		private readonly bool _safeAsync;

		private readonly TextWriter _writer;

		private Newtonsoft.Json.Utilities.Base64Encoder _base64Encoder;

		private char _indentChar;

		private int _indentation;

		private char _quoteChar;

		private bool _quoteName;

		private bool[] _charEscapeFlags;

		private char[] _writeBuffer;

		private IArrayPool<char> _arrayPool;

		private char[] _indentChars;

		public IArrayPool<char> ArrayPool
		{
			get
			{
				return this._arrayPool;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._arrayPool = value;
			}
		}

		private Newtonsoft.Json.Utilities.Base64Encoder Base64Encoder
		{
			get
			{
				if (this._base64Encoder == null)
				{
					this._base64Encoder = new Newtonsoft.Json.Utilities.Base64Encoder(this._writer);
				}
				return this._base64Encoder;
			}
		}

		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				if (value != this._indentChar)
				{
					this._indentChar = value;
					this._indentChars = null;
				}
			}
		}

		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '\"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
				this.UpdateCharEscapeFlags();
			}
		}

		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		public JsonTextWriter(TextWriter textWriter)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '\"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
			this.UpdateCharEscapeFlags();
			this._safeAsync = base.GetType() == typeof(JsonTextWriter);
		}

		public override void Close()
		{
			base.Close();
			this.CloseBufferAndWriter();
		}

		public override Task CloseAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.CloseAsync(cancellationToken);
			}
			return this.DoCloseAsync(cancellationToken);
		}

		private void CloseBufferAndWriter()
		{
			if (this._writeBuffer != null)
			{
				BufferUtils.ReturnBuffer(this._arrayPool, this._writeBuffer);
				this._writeBuffer = null;
			}
			if (base.CloseOutput)
			{
				TextWriter textWriter = this._writer;
				if (textWriter == null)
				{
					return;
				}
				textWriter.Close();
			}
		}

		internal async Task DoCloseAsync(CancellationToken cancellationToken)
		{
			JsonTextWriter.<DoCloseAsync>d__8 variable = new JsonTextWriter.<DoCloseAsync>d__8();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextWriter.<DoCloseAsync>d__8>(ref variable);
			return variable.<>t__builder.Task;
		}

		internal Task DoFlushAsync(CancellationToken cancellationToken)
		{
			return cancellationToken.CancelIfRequestedAsync() ?? this._writer.FlushAsync();
		}

		internal async Task DoWriteCommentAsync(string text, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteCommentAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync("/*", cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(text, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync("*/", cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteEndAsync(JsonToken token, CancellationToken cancellationToken)
		{
			switch (token)
			{
				case JsonToken.EndObject:
				{
					return this._writer.WriteAsync('}', cancellationToken);
				}
				case JsonToken.EndArray:
				{
					return this._writer.WriteAsync(']', cancellationToken);
				}
				case JsonToken.EndConstructor:
				{
					return this._writer.WriteAsync(')', cancellationToken);
				}
				default:
				{
					throw JsonWriterException.Create(this, string.Concat("Invalid JsonToken: ", token), null);
				}
			}
		}

		internal Task DoWriteIndentAsync(CancellationToken cancellationToken)
		{
			int top = base.Top * this._indentation;
			int num = this.SetIndentChars();
			if (top > 12)
			{
				return this.WriteIndentAsync(top, num, cancellationToken);
			}
			return this._writer.WriteAsync(this._indentChars, 0, num + top, cancellationToken);
		}

		internal Task DoWriteIndentSpaceAsync(CancellationToken cancellationToken)
		{
			return this._writer.WriteAsync(' ', cancellationToken);
		}

		internal Task DoWriteNullAsync(CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Null, JsonConvert.Null, cancellationToken);
		}

		internal Task DoWritePropertyNameAsync(string name, CancellationToken cancellationToken)
		{
			Task task = base.InternalWritePropertyNameAsync(name, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.DoWritePropertyNameAsync(task, name, cancellationToken);
			}
			task = this.WriteEscapedStringAsync(name, this._quoteName, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this._writer.WriteAsync(':', cancellationToken);
			}
			return JavaScriptUtils.WriteCharAsync(task, this._writer, ':', cancellationToken);
		}

		private async Task DoWritePropertyNameAsync(Task task, string name, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteEscapedStringAsync(name, this._quoteName, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(':').ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal async Task DoWritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWritePropertyNameAsync(name, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (!escape)
			{
				if (this._quoteName)
				{
					configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
					await configuredTaskAwaitable;
				}
				configuredTaskAwaitable = this._writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				if (this._quoteName)
				{
					configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
					await configuredTaskAwaitable;
				}
			}
			else
			{
				configuredTaskAwaitable = this.WriteEscapedStringAsync(name, this._quoteName, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			configuredTaskAwaitable = this._writer.WriteAsync(':').ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteRawAsync(string json, CancellationToken cancellationToken)
		{
			return this._writer.WriteAsync(json, cancellationToken);
		}

		internal Task DoWriteRawValueAsync(string json, CancellationToken cancellationToken)
		{
			base.UpdateScopeWithFinishedValue();
			Task task = base.AutoCompleteAsync(JsonToken.Undefined, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this.WriteRawAsync(json, cancellationToken);
			}
			return this.DoWriteRawValueAsync(task, json, cancellationToken);
		}

		private async Task DoWriteRawValueAsync(Task task, string json, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteRawAsync(json, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteStartArrayAsync(CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteStartAsync(JsonToken.StartArray, JsonContainerType.Array, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.DoWriteStartArrayAsync(task, cancellationToken);
			}
			return this._writer.WriteAsync('[', cancellationToken);
		}

		internal async Task DoWriteStartArrayAsync(Task task, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync('[', cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal async Task DoWriteStartConstructorAsync(string name, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteStartAsync(JsonToken.StartConstructor, JsonContainerType.Constructor, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync("new ", cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync('(').ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteStartObjectAsync(CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteStartAsync(JsonToken.StartObject, JsonContainerType.Object, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.DoWriteStartObjectAsync(task, cancellationToken);
			}
			return this._writer.WriteAsync('{', cancellationToken);
		}

		internal async Task DoWriteStartObjectAsync(Task task, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync('{', cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteUndefinedAsync(CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteValueAsync(JsonToken.Undefined, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.DoWriteUndefinedAsync(task, cancellationToken);
			}
			return this._writer.WriteAsync(JsonConvert.Undefined, cancellationToken);
		}

		private async Task DoWriteUndefinedAsync(Task task, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(JsonConvert.Undefined, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteValueAsync(bool value, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Boolean, JsonConvert.ToString(value), cancellationToken);
		}

		internal Task DoWriteValueAsync(bool? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(byte? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value.GetValueOrDefault()), cancellationToken);
		}

		internal Task DoWriteValueAsync(char value, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.String, JsonConvert.ToString(value), cancellationToken);
		}

		internal Task DoWriteValueAsync(char? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal async Task DoWriteValueAsync(DateTime value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (!string.IsNullOrEmpty(base.DateFormatString))
			{
				configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
				await configuredTaskAwaitable;
				configuredTaskAwaitable = this._writer.WriteAsync(value.ToString(base.DateFormatString, base.Culture), cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			else
			{
				int buffer = this.WriteValueToBuffer(value);
				configuredTaskAwaitable = this._writer.WriteAsync(this._writeBuffer, 0, buffer, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
		}

		internal Task DoWriteValueAsync(DateTime? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal async Task DoWriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (!string.IsNullOrEmpty(base.DateFormatString))
			{
				configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
				await configuredTaskAwaitable;
				configuredTaskAwaitable = this._writer.WriteAsync(value.ToString(base.DateFormatString, base.Culture), cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			else
			{
				int buffer = this.WriteValueToBuffer(value);
				configuredTaskAwaitable = this._writer.WriteAsync(this._writeBuffer, 0, buffer, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
		}

		internal Task DoWriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(decimal value, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value), cancellationToken);
		}

		internal Task DoWriteValueAsync(decimal? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal async Task DoWriteValueAsync(Guid value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(value.ToString("D", CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteValueAsync(Guid? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(int? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(long? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(sbyte? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(short? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(string value, CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteValueAsync(JsonToken.String, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.DoWriteValueAsync(task, value, cancellationToken);
			}
			if (value != null)
			{
				return this.WriteEscapedStringAsync(value, true, cancellationToken);
			}
			return this._writer.WriteAsync(JsonConvert.Null, cancellationToken);
		}

		private async Task DoWriteValueAsync(Task task, string value, CancellationToken cancellationToken)
		{
			Task task1;
			await task.ConfigureAwait(false);
			task1 = (value == null ? this._writer.WriteAsync(JsonConvert.Null, cancellationToken) : this.WriteEscapedStringAsync(value, true, cancellationToken));
			await task1.ConfigureAwait(false);
		}

		internal async Task DoWriteValueAsync(TimeSpan value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(value.ToString(null, CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task DoWriteValueAsync(TimeSpan? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(uint? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value.GetValueOrDefault()), cancellationToken);
		}

		internal Task DoWriteValueAsync(ulong? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
		}

		internal Task DoWriteValueAsync(ushort? value, CancellationToken cancellationToken)
		{
			if (!value.HasValue)
			{
				return this.DoWriteNullAsync(cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value.GetValueOrDefault()), cancellationToken);
		}

		internal Task DoWriteValueDelimiterAsync(CancellationToken cancellationToken)
		{
			return this._writer.WriteAsync(',', cancellationToken);
		}

		internal Task DoWriteWhitespaceAsync(string ws, CancellationToken cancellationToken)
		{
			base.InternalWriteWhitespace(ws);
			return this._writer.WriteAsync(ws, cancellationToken);
		}

		internal char[] EnsureWriteBuffer(int length, int copyTo)
		{
			if (length < 35)
			{
				length = 35;
			}
			char[] chrArray = this._writeBuffer;
			if (chrArray == null)
			{
				char[] chrArray1 = BufferUtils.RentBuffer(this._arrayPool, length);
				char[] chrArray2 = chrArray1;
				this._writeBuffer = chrArray1;
				return chrArray2;
			}
			if ((int)chrArray.Length >= length)
			{
				return chrArray;
			}
			char[] chrArray3 = BufferUtils.RentBuffer(this._arrayPool, length);
			if (copyTo != 0)
			{
				Array.Copy(chrArray, chrArray3, copyTo);
			}
			BufferUtils.ReturnBuffer(this._arrayPool, chrArray);
			this._writeBuffer = chrArray3;
			return chrArray3;
		}

		private void EnsureWriteBuffer()
		{
			if (this._writeBuffer == null)
			{
				this._writeBuffer = BufferUtils.RentBuffer(this._arrayPool, 35);
			}
		}

		public override void Flush()
		{
			this._writer.Flush();
		}

		public override Task FlushAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.FlushAsync(cancellationToken);
			}
			return this.DoFlushAsync(cancellationToken);
		}

		internal override void OnStringEscapeHandlingChanged()
		{
			this.UpdateCharEscapeFlags();
		}

		private int SetIndentChars()
		{
			bool flag;
			string newLine = this._writer.NewLine;
			int length = newLine.Length;
			flag = (this._indentChars == null ? false : (int)this._indentChars.Length == 12 + length);
			bool flag1 = flag;
			if (flag)
			{
				int num = 0;
				while (num != length)
				{
					if (newLine[num] == this._indentChars[num])
					{
						num++;
					}
					else
					{
						flag1 = false;
						if (!flag1)
						{
							this._indentChars = string.Concat(newLine, new string(this._indentChar, 12)).ToCharArray();
						}
						return length;
					}
				}
			}
			if (!flag1)
			{
				this._indentChars = string.Concat(newLine, new string(this._indentChar, 12)).ToCharArray();
			}
			return length;
		}

		private void UpdateCharEscapeFlags()
		{
			this._charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, this._quoteChar);
		}

		public override void WriteComment(string text)
		{
			base.InternalWriteComment();
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		public override Task WriteCommentAsync(string text, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteCommentAsync(text, cancellationToken);
			}
			return this.DoWriteCommentAsync(text, cancellationToken);
		}

		private Task WriteDigitsAsync(ulong uvalue, bool negative, CancellationToken cancellationToken)
		{
			if (uvalue <= 9L & !negative)
			{
				return this._writer.WriteAsync((char)(48L + uvalue), cancellationToken);
			}
			int buffer = this.WriteNumberToBuffer(uvalue, negative);
			return this._writer.WriteAsync(this._writeBuffer, 0, buffer, cancellationToken);
		}

		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
				case JsonToken.EndObject:
				{
					this._writer.Write('}');
					return;
				}
				case JsonToken.EndArray:
				{
					this._writer.Write(']');
					return;
				}
				case JsonToken.EndConstructor:
				{
					this._writer.Write(')');
					return;
				}
				default:
				{
					throw JsonWriterException.Create(this, string.Concat("Invalid JsonToken: ", token), null);
				}
			}
		}

		public override Task WriteEndArrayAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteEndArrayAsync(cancellationToken);
			}
			return base.InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);
		}

		protected override Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (!this._safeAsync)
			{
				return base.WriteEndAsync(token, cancellationToken);
			}
			return this.DoWriteEndAsync(token, cancellationToken);
		}

		public override Task WriteEndAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteEndAsync(cancellationToken);
			}
			return base.WriteEndInternalAsync(cancellationToken);
		}

		public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteEndConstructorAsync(cancellationToken);
			}
			return base.InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);
		}

		public override Task WriteEndObjectAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteEndObjectAsync(cancellationToken);
			}
			return base.InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);
		}

		private void WriteEscapedString(string value, bool quote)
		{
			this.EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, base.StringEscapeHandling, this._arrayPool, ref this._writeBuffer);
		}

		private Task WriteEscapedStringAsync(string value, bool quote, CancellationToken cancellationToken)
		{
			return JavaScriptUtils.WriteEscapedJavaScriptStringAsync(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, base.StringEscapeHandling, this, this._writeBuffer, cancellationToken);
		}

		protected override void WriteIndent()
		{
			int top = base.Top * this._indentation;
			int num = this.SetIndentChars();
			this._writer.Write(this._indentChars, 0, num + Math.Min(top, 12));
			while (true)
			{
				int num1 = top - 12;
				top = num1;
				if (num1 <= 0)
				{
					break;
				}
				this._writer.Write(this._indentChars, num, Math.Min(top, 12));
			}
		}

		protected override Task WriteIndentAsync(CancellationToken cancellationToken)
		{
			if (!this._safeAsync)
			{
				return base.WriteIndentAsync(cancellationToken);
			}
			return this.DoWriteIndentAsync(cancellationToken);
		}

		private async Task WriteIndentAsync(int currentIndentCount, int newLineLen, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = this._writer.WriteAsync(this._indentChars, 0, newLineLen + Math.Min(currentIndentCount, 12), cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			while (true)
			{
				int num = currentIndentCount - 12;
				int num1 = num;
				currentIndentCount = num;
				if (num1 <= 0)
				{
					break;
				}
				configuredTaskAwaitable = this._writer.WriteAsync(this._indentChars, newLineLen, Math.Min(currentIndentCount, 12), cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
		}

		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		protected override Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
		{
			if (!this._safeAsync)
			{
				return base.WriteIndentSpaceAsync(cancellationToken);
			}
			return this.DoWriteIndentSpaceAsync(cancellationToken);
		}

		private void WriteIntegerValue(long value)
		{
			if (value >= 0L && value <= 9L)
			{
				this._writer.Write((char)(48L + value));
				return;
			}
			bool flag = value < 0L;
			this.WriteIntegerValue((flag ? (ulong)(-value) : (ulong)value), flag);
		}

		private void WriteIntegerValue(ulong value, bool negative)
		{
			if (!negative & value <= 9L)
			{
				this._writer.Write((char)(48L + value));
				return;
			}
			int buffer = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, buffer);
		}

		private void WriteIntegerValue(int value)
		{
			if (value >= 0 && value <= 9)
			{
				this._writer.Write((char)(48 + value));
				return;
			}
			bool flag = value < 0;
			this.WriteIntegerValue((flag ? (uint)(-value) : (uint)value), flag);
		}

		private void WriteIntegerValue(uint value, bool negative)
		{
			if (!negative & value <= 9)
			{
				this._writer.Write((char)(48 + value));
				return;
			}
			int buffer = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, buffer);
		}

		private Task WriteIntegerValueAsync(ulong uvalue, bool negative, CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteValueAsync(JsonToken.Integer, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this.WriteDigitsAsync(uvalue, negative, cancellationToken);
			}
			return this.WriteIntegerValueAsync(task, uvalue, negative, cancellationToken);
		}

		private async Task WriteIntegerValueAsync(Task task, ulong uvalue, bool negative, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteDigitsAsync(uvalue, negative, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task WriteIntegerValueAsync(long value, CancellationToken cancellationToken)
		{
			bool flag = value < 0L;
			bool flag1 = flag;
			if (flag)
			{
				value = -value;
			}
			return this.WriteIntegerValueAsync((ulong)value, flag1, cancellationToken);
		}

		internal Task WriteIntegerValueAsync(ulong uvalue, CancellationToken cancellationToken)
		{
			return this.WriteIntegerValueAsync(uvalue, false, cancellationToken);
		}

		public override void WriteNull()
		{
			base.InternalWriteValue(JsonToken.Null);
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		public override Task WriteNullAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteNullAsync(cancellationToken);
			}
			return this.DoWriteNullAsync(cancellationToken);
		}

		private int WriteNumberToBuffer(ulong value, bool negative)
		{
			if (value <= 4294967295L)
			{
				return this.WriteNumberToBuffer((uint)value, negative);
			}
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength(value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num1 = num;
			do
			{
				ulong num2 = value / 10L;
				ulong num3 = value - num2 * 10L;
				int num4 = num1 - 1;
				num1 = num4;
				this._writeBuffer[num4] = (char)(48L + num3);
				value = num2;
			}
			while (value != 0);
			return num;
		}

		private int WriteNumberToBuffer(uint value, bool negative)
		{
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength((ulong)value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num1 = num;
			do
			{
				uint num2 = value / 10;
				uint num3 = value - num2 * 10;
				int num4 = num1 - 1;
				num1 = num4;
				this._writeBuffer[num4] = (char)(48 + num3);
				value = num2;
			}
			while (value != 0);
			return num;
		}

		public override void WritePropertyName(string name)
		{
			base.InternalWritePropertyName(name);
			this.WriteEscapedString(name, this._quoteName);
			this._writer.Write(':');
		}

		public override void WritePropertyName(string name, bool escape)
		{
			base.InternalWritePropertyName(name);
			if (!escape)
			{
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
				this._writer.Write(name);
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
			}
			else
			{
				this.WriteEscapedString(name, this._quoteName);
			}
			this._writer.Write(':');
		}

		public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WritePropertyNameAsync(name, cancellationToken);
			}
			return this.DoWritePropertyNameAsync(name, cancellationToken);
		}

		public override Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WritePropertyNameAsync(name, escape, cancellationToken);
			}
			return this.DoWritePropertyNameAsync(name, escape, cancellationToken);
		}

		public override void WriteRaw(string json)
		{
			base.InternalWriteRaw();
			this._writer.Write(json);
		}

		public override Task WriteRawAsync(string json, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteRawAsync(json, cancellationToken);
			}
			return this.DoWriteRawAsync(json, cancellationToken);
		}

		public override Task WriteRawValueAsync(string json, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteRawValueAsync(json, cancellationToken);
			}
			return this.DoWriteRawValueAsync(json, cancellationToken);
		}

		public override void WriteStartArray()
		{
			base.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			this._writer.Write('[');
		}

		public override Task WriteStartArrayAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteStartArrayAsync(cancellationToken);
			}
			return this.DoWriteStartArrayAsync(cancellationToken);
		}

		public override void WriteStartConstructor(string name)
		{
			base.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write('(');
		}

		public override Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteStartConstructorAsync(name, cancellationToken);
			}
			return this.DoWriteStartConstructorAsync(name, cancellationToken);
		}

		public override void WriteStartObject()
		{
			base.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			this._writer.Write('{');
		}

		public override Task WriteStartObjectAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteStartObjectAsync(cancellationToken);
			}
			return this.DoWriteStartObjectAsync(cancellationToken);
		}

		public override void WriteUndefined()
		{
			base.InternalWriteValue(JsonToken.Undefined);
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		public override Task WriteUndefinedAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteUndefinedAsync(cancellationToken);
			}
			return this.DoWriteUndefinedAsync(cancellationToken);
		}

		public override void WriteValue(object value)
		{
			object obj = value;
			object obj1 = obj;
			if (!(obj is BigInteger))
			{
				base.WriteValue(value);
				return;
			}
			BigInteger bigInteger = (BigInteger)obj1;
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteValueInternal(bigInteger.ToString(CultureInfo.InvariantCulture), JsonToken.String);
		}

		public override void WriteValue(string value)
		{
			base.InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
				return;
			}
			this.WriteEscapedString(value, true);
		}

		public override void WriteValue(int value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)value);
		}

		public override void WriteValue(long value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value, false);
		}

		public override void WriteValue(float value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		public override void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		public override void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			base.InternalWriteValue(JsonToken.Boolean);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		public override void WriteValue(char value)
		{
			base.InternalWriteValue(JsonToken.String);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		public override void WriteValue(byte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		public override void WriteValue(decimal value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		public override void WriteValue(DateTime value)
		{
			base.InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				int buffer = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, buffer);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Bytes);
			this._writer.Write(this._quoteChar);
			this.Base64Encoder.Encode(value, 0, (int)value.Length);
			this.Base64Encoder.Flush();
			this._writer.Write(this._quoteChar);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			base.InternalWriteValue(JsonToken.Date);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				int buffer = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, buffer);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		public override void WriteValue(Guid value)
		{
			base.InternalWriteValue(JsonToken.String);
			string str = null;
			str = value.ToString("D", CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(str);
			this._writer.Write(this._quoteChar);
		}

		public override void WriteValue(TimeSpan value)
		{
			base.InternalWriteValue(JsonToken.String);
			string str = value.ToString(null, CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(str);
			this._writer.Write(this._quoteChar);
		}

		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
			this.WriteEscapedString(value.OriginalString, true);
		}

		public override Task WriteValueAsync(bool value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(bool? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(byte value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value), cancellationToken);
		}

		public override Task WriteValueAsync(byte? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			if (value == null)
			{
				return this.WriteNullAsync(cancellationToken);
			}
			return this.WriteValueNonNullAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(char value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(char? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(decimal value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(double value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteValueAsync(value, false, cancellationToken);
		}

		internal Task WriteValueAsync(double value, bool nullable, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, nullable), cancellationToken);
		}

		public override Task WriteValueAsync(double? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			if (!value.HasValue)
			{
				return this.WriteNullAsync(cancellationToken);
			}
			return this.WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken);
		}

		public override Task WriteValueAsync(float value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteValueAsync(value, false, cancellationToken);
		}

		internal Task WriteValueAsync(float value, bool nullable, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, nullable), cancellationToken);
		}

		public override Task WriteValueAsync(float? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			if (!value.HasValue)
			{
				return this.WriteNullAsync(cancellationToken);
			}
			return this.WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken);
		}

		public override Task WriteValueAsync(Guid value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(int value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value, cancellationToken);
		}

		public override Task WriteValueAsync(int? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(long value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(long? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		internal Task WriteValueAsync(BigInteger value, CancellationToken cancellationToken)
		{
			return this.WriteValueInternalAsync(JsonToken.Integer, value.ToString(CultureInfo.InvariantCulture), cancellationToken);
		}

		public override Task WriteValueAsync(object value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			if (value == null)
			{
				return this.WriteNullAsync(cancellationToken);
			}
			object obj = value;
			object obj1 = obj;
			if (obj is BigInteger)
			{
				return this.WriteValueAsync((BigInteger)obj1, cancellationToken);
			}
			return JsonWriter.WriteValueAsync(this, ConvertUtils.GetTypeCode(value.GetType()), value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(short value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)value, cancellationToken);
		}

		public override Task WriteValueAsync(short? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(string value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(uint value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value), cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(uint? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(ulong value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync(value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(Uri value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			if (value == null)
			{
				return this.WriteNullAsync(cancellationToken);
			}
			return this.WriteValueNotNullAsync(value, cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(ushort value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.WriteIntegerValueAsync((long)((ulong)value), cancellationToken);
		}

		[CLSCompliant(false)]
		public override Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueAsync(value, cancellationToken);
			}
			return this.DoWriteValueAsync(value, cancellationToken);
		}

		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		protected override Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
		{
			if (!this._safeAsync)
			{
				return base.WriteValueDelimiterAsync(cancellationToken);
			}
			return this.DoWriteValueDelimiterAsync(cancellationToken);
		}

		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		private Task WriteValueInternalAsync(JsonToken token, string value, CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteValueAsync(token, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this._writer.WriteAsync(value, cancellationToken);
			}
			return this.WriteValueInternalAsync(task, value, cancellationToken);
		}

		private async Task WriteValueInternalAsync(Task task, string value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(value, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal async Task WriteValueNonNullAsync(byte[] value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.InternalWriteValueAsync(JsonToken.Bytes, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.Base64Encoder.EncodeAsync(value, 0, (int)value.Length, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.Base64Encoder.FlushAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this._writer.WriteAsync(this._quoteChar).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		internal Task WriteValueNotNullAsync(Uri value, CancellationToken cancellationToken)
		{
			Task task = base.InternalWriteValueAsync(JsonToken.String, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.WriteValueNotNullAsync(task, value, cancellationToken);
			}
			return this.WriteEscapedStringAsync(value.OriginalString, true, cancellationToken);
		}

		internal async Task WriteValueNotNullAsync(Task task, Uri value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteEscapedStringAsync(value.OriginalString, true, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		private int WriteValueToBuffer(DateTime value)
		{
			this.EnsureWriteBuffer();
			int num = 0;
			num = 1;
			this._writeBuffer[0] = this._quoteChar;
			TimeSpan? nullable = null;
			num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, 1, value, nullable, value.Kind, base.DateFormatHandling);
			int num1 = num;
			num = num1 + 1;
			this._writeBuffer[num1] = this._quoteChar;
			return num;
		}

		private int WriteValueToBuffer(DateTimeOffset value)
		{
			this.EnsureWriteBuffer();
			int num = 0;
			num = 1;
			this._writeBuffer[0] = this._quoteChar;
			num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, 1, (base.DateFormatHandling == Newtonsoft.Json.DateFormatHandling.IsoDateFormat ? value.DateTime : value.UtcDateTime), new TimeSpan?(value.Offset), DateTimeKind.Local, base.DateFormatHandling);
			int num1 = num;
			num = num1 + 1;
			this._writeBuffer[num1] = this._quoteChar;
			return num;
		}

		public override void WriteWhitespace(string ws)
		{
			base.InternalWriteWhitespace(ws);
			this._writer.Write(ws);
		}

		public override Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.WriteWhitespaceAsync(ws, cancellationToken);
			}
			return this.DoWriteWhitespaceAsync(ws, cancellationToken);
		}
	}
}