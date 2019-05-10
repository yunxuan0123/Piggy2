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
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		private readonly bool _safeAsync;

		private readonly TextReader _reader;

		private char[] _chars;

		private int _charsUsed;

		private int _charPos;

		private int _lineStartPos;

		private int _lineNumber;

		private bool _isEndOfFile;

		private StringBuffer _stringBuffer;

		private StringReference _stringReference;

		private IArrayPool<char> _arrayPool;

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

		public int LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start && this.LinePosition == 0 && this.TokenType != JsonToken.Comment)
				{
					return 0;
				}
				return this._lineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return this._charPos - this._lineStartPos;
			}
		}

		public JsonNameTable PropertyNameTable
		{
			get;
			set;
		}

		public JsonTextReader(TextReader reader)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this._lineNumber = 1;
			this._safeAsync = base.GetType() == typeof(JsonTextReader);
		}

		private static object BigIntegerParse(string number, CultureInfo culture)
		{
			return BigInteger.Parse(number, culture);
		}

		private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
		}

		private void ClearRecentString()
		{
			this._stringBuffer.Position = 0;
			this._stringReference = new StringReference();
		}

		public override void Close()
		{
			base.Close();
			if (this._chars != null)
			{
				BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
				this._chars = null;
			}
			if (base.CloseInput)
			{
				TextReader textReader = this._reader;
				if (textReader != null)
				{
					textReader.Close();
				}
				else
				{
				}
			}
			this._stringBuffer.Clear(this._arrayPool);
		}

		private char ConvertUnicode(bool enoughChars)
		{
			int num;
			if (!enoughChars)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing Unicode escape sequence.");
			}
			if (!ConvertUtils.TryHexTextToInt(this._chars, this._charPos, this._charPos + 4, out num))
			{
				throw JsonReaderException.Create(this, "Invalid Unicode escape sequence: \\u{0}.".FormatWith(CultureInfo.InvariantCulture, new string(this._chars, this._charPos, 4)));
			}
			this._charPos += 4;
			return Convert.ToChar(num);
		}

		private JsonReaderException CreateUnexpectedCharacterException(char c)
		{
			return JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		internal async Task<bool?> DoReadAsBooleanAsync(CancellationToken cancellationToken)
		{
			JsonTextReader.<DoReadAsBooleanAsync>d__40 variable = new JsonTextReader.<DoReadAsBooleanAsync>d__40();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool?>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<DoReadAsBooleanAsync>d__40>(ref variable);
			return variable.<>t__builder.Task;
		}

		internal async Task<byte[]> DoReadAsBytesAsync(CancellationToken cancellationToken)
		{
			JsonTextReader.<DoReadAsBytesAsync>d__42 variable = new JsonTextReader.<DoReadAsBytesAsync>d__42();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<DoReadAsBytesAsync>d__42>(ref variable);
			return variable.<>t__builder.Task;
		}

		internal async Task<DateTime?> DoReadAsDateTimeAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadStringValueAsync(ReadType.ReadAsDateTime, cancellationToken).ConfigureAwait(false);
			return (DateTime?)await configuredTaskAwaitable;
		}

		internal async Task<DateTimeOffset?> DoReadAsDateTimeOffsetAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadStringValueAsync(ReadType.ReadAsDateTimeOffset, cancellationToken).ConfigureAwait(false);
			return (DateTimeOffset?)await configuredTaskAwaitable;
		}

		internal async Task<decimal?> DoReadAsDecimalAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadNumberValueAsync(ReadType.ReadAsDecimal, cancellationToken).ConfigureAwait(false);
			return (decimal?)await configuredTaskAwaitable;
		}

		internal async Task<double?> DoReadAsDoubleAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadNumberValueAsync(ReadType.ReadAsDouble, cancellationToken).ConfigureAwait(false);
			return (double?)await configuredTaskAwaitable;
		}

		internal async Task<int?> DoReadAsInt32Async(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadNumberValueAsync(ReadType.const_1, cancellationToken).ConfigureAwait(false);
			return (int?)await configuredTaskAwaitable;
		}

		internal async Task<string> DoReadAsStringAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable = this.ReadStringValueAsync(ReadType.ReadAsString, cancellationToken).ConfigureAwait(false);
			return (string)await configuredTaskAwaitable;
		}

		internal Task<bool> DoReadAsync(CancellationToken cancellationToken)
		{
			Task<bool> task;
			this.EnsureBuffer();
			do
			{
				switch (this._currentState)
				{
					case JsonReader.State.Start:
					case JsonReader.State.Property:
					case JsonReader.State.ArrayStart:
					case JsonReader.State.Array:
					case JsonReader.State.ConstructorStart:
					case JsonReader.State.Constructor:
					{
						return this.ParseValueAsync(cancellationToken);
					}
					case JsonReader.State.Complete:
					case JsonReader.State.Closed:
					case JsonReader.State.Error:
					{
						throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
					}
					case JsonReader.State.ObjectStart:
					case JsonReader.State.Object:
					{
						return this.ParseObjectAsync(cancellationToken);
					}
					case JsonReader.State.PostValue:
					{
						task = this.ParsePostValueAsync(false, cancellationToken);
						if (task.IsCompletedSucessfully())
						{
							continue;
						}
						return this.DoReadAsync(task, cancellationToken);
					}
					case JsonReader.State.Finished:
					{
						return this.ReadFromFinishedAsync(cancellationToken);
					}
					default:
					{
						throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
					}
				}
			}
			while (!task.Result);
			return AsyncUtils.True;
		}

		private async Task<bool> DoReadAsync(Task<bool> task, CancellationToken cancellationToken)
		{
			bool flag;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = task.ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				configuredTaskAwaitable = this.DoReadAsync(cancellationToken).ConfigureAwait(false);
				flag = await configuredTaskAwaitable;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		private void EatWhitespace()
		{
			do
			{
			Label0:
				char chr = this._chars[this._charPos];
				if (chr == 0)
				{
					if (this._charsUsed == this._charPos)
					{
						continue;
					}
					this._charPos++;
					goto Label0;
				}
				else if (chr == '\n')
				{
					this.ProcessLineFeed();
					goto Label0;
				}
				else if (chr == '\r')
				{
					this.ProcessCarriageReturn(false);
					goto Label0;
				}
				else
				{
					if (chr != ' ' && !char.IsWhiteSpace(chr))
					{
						return;
					}
					this._charPos++;
					goto Label0;
				}
			}
			while (this.ReadData(false) != 0);
		}

		private async Task EatWhitespaceAsync(CancellationToken cancellationToken)
		{
			JsonTextReader.<EatWhitespaceAsync>d__17 variable = new JsonTextReader.<EatWhitespaceAsync>d__17();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<EatWhitespaceAsync>d__17>(ref variable);
			return variable.<>t__builder.Task;
		}

		private void EndComment(bool setToken, int initialPosition, int endPosition)
		{
			if (setToken)
			{
				base.SetToken(JsonToken.Comment, new string(this._chars, initialPosition, endPosition - initialPosition));
			}
		}

		private void EnsureBuffer()
		{
			if (this._chars == null)
			{
				this._chars = BufferUtils.RentBuffer(this._arrayPool, 1024);
				this._chars[0] = '\0';
			}
		}

		private void EnsureBufferNotEmpty()
		{
			if (this._stringBuffer.IsEmpty)
			{
				this._stringBuffer = new StringBuffer(this._arrayPool, 1024);
			}
		}

		private bool EnsureChars(int relativePosition, bool append)
		{
			if (this._charPos + relativePosition < this._charsUsed)
			{
				return true;
			}
			return this.ReadChars(relativePosition, append);
		}

		private Task<bool> EnsureCharsAsync(int relativePosition, bool append, CancellationToken cancellationToken)
		{
			if (this._charPos + relativePosition < this._charsUsed)
			{
				return AsyncUtils.True;
			}
			if (this._isEndOfFile)
			{
				return AsyncUtils.False;
			}
			return this.ReadCharsAsync(relativePosition, append, cancellationToken);
		}

		private object FinishReadQuotedNumber(ReadType readType)
		{
			if (readType == ReadType.const_1)
			{
				return base.ReadInt32String(this._stringReference.ToString());
			}
			if (readType == ReadType.ReadAsDecimal)
			{
				return base.ReadDecimalString(this._stringReference.ToString());
			}
			if (readType != ReadType.ReadAsDouble)
			{
				throw new ArgumentOutOfRangeException("readType");
			}
			return base.ReadDoubleString(this._stringReference.ToString());
		}

		private object FinishReadQuotedStringValue(ReadType readType)
		{
			object obj;
			switch (readType)
			{
				case ReadType.ReadAsBytes:
				case ReadType.ReadAsString:
				{
					return this.Value;
				}
				case ReadType.ReadAsDecimal:
				{
					throw new ArgumentOutOfRangeException("readType");
				}
				case ReadType.ReadAsDateTime:
				{
					object value = this.Value;
					obj = value;
					if (value is DateTime)
					{
						return (DateTime)obj;
					}
					return base.ReadDateTimeString((string)this.Value);
				}
				case ReadType.ReadAsDateTimeOffset:
				{
					object value1 = this.Value;
					obj = value1;
					if (value1 is DateTimeOffset)
					{
						return (DateTimeOffset)obj;
					}
					return base.ReadDateTimeOffsetString((string)this.Value);
				}
				default:
				{
					throw new ArgumentOutOfRangeException("readType");
				}
			}
		}

		private void FinishReadStringIntoBuffer(int charPos, int initialPosition, int lastWritePosition)
		{
			if (initialPosition != lastWritePosition)
			{
				this.EnsureBufferNotEmpty();
				if (charPos > lastWritePosition)
				{
					this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, charPos - lastWritePosition);
				}
				this._stringReference = new StringReference(this._stringBuffer.InternalBuffer, 0, this._stringBuffer.Position);
			}
			else
			{
				this._stringReference = new StringReference(this._chars, initialPosition, charPos - initialPosition);
			}
			this._charPos = charPos + 1;
		}

		private void HandleNull()
		{
			if (!this.EnsureChars(1, true))
			{
				this._charPos = this._charsUsed;
				throw base.CreateUnexpectedEndException();
			}
			if (this._chars[this._charPos + 1] != 'u')
			{
				this._charPos += 2;
				throw this.CreateUnexpectedCharacterException(this._chars[this._charPos - 1]);
			}
			this.ParseNull();
		}

		private async Task HandleNullAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				this._charPos = this._charsUsed;
				throw base.CreateUnexpectedEndException();
			}
			if (this._chars[this._charPos + 1] != 'u')
			{
				this._charPos += 2;
				throw this.CreateUnexpectedCharacterException(this._chars[this._charPos - 1]);
			}
			await this.ParseNullAsync(cancellationToken).ConfigureAwait(false);
		}

		public bool HasLineInfo()
		{
			return true;
		}

		private bool IsSeparator(char c)
		{
			if (c > ')')
			{
				if (c <= '/')
				{
					if (c == ',')
					{
						return true;
					}
					if (c == '/')
					{
						if (!this.EnsureChars(1, false))
						{
							return false;
						}
						char chr = this._chars[this._charPos + 1];
						if (chr == '*')
						{
							return true;
						}
						return chr == '/';
					}
					if (char.IsWhiteSpace(c))
					{
						return true;
					}
					return false;
				}
				else if (c != ']')
				{
					if (c != '}')
					{
						if (char.IsWhiteSpace(c))
						{
							return true;
						}
						return false;
					}
				}
				return true;
			}
			else
			{
				switch (c)
				{
					case '\t':
					case '\n':
					case '\r':
					{
						return true;
					}
					case '\v':
					case '\f':
					{
						break;
					}
					default:
					{
						if (c == ' ')
						{
							return true;
						}
						if (c == ')')
						{
							if (base.CurrentState != JsonReader.State.Constructor && base.CurrentState != JsonReader.State.ConstructorStart)
							{
								return false;
							}
							return true;
						}
						else
						{
							break;
						}
					}
				}
			}
			if (char.IsWhiteSpace(c))
			{
				return true;
			}
			return false;
		}

		private async Task MatchAndSetAsync(string value, JsonToken newToken, object tokenValue, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueWithTrailingSeparatorAsync(value, cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				throw JsonReaderException.Create(this, string.Concat("Error parsing ", newToken.ToString().ToLowerInvariant(), " value."));
			}
			base.SetToken(newToken, tokenValue);
		}

		private bool MatchValue(string value)
		{
			return this.MatchValue(this.EnsureChars(value.Length - 1, true), value);
		}

		private bool MatchValue(bool enoughChars, string value)
		{
			if (!enoughChars)
			{
				this._charPos = this._charsUsed;
				throw base.CreateUnexpectedEndException();
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (this._chars[this._charPos + i] != value[i])
				{
					this._charPos += i;
					return false;
				}
			}
			this._charPos += value.Length;
			return true;
		}

		private async Task<bool> MatchValueAsync(string value, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.EnsureCharsAsync(value.Length - 1, true, cancellationToken).ConfigureAwait(false);
			return this.MatchValue(await configuredTaskAwaitable, value);
		}

		private bool MatchValueWithTrailingSeparator(string value)
		{
			if (!this.MatchValue(value))
			{
				return false;
			}
			if (!this.EnsureChars(0, false))
			{
				return true;
			}
			if (this.IsSeparator(this._chars[this._charPos]))
			{
				return true;
			}
			return this._chars[this._charPos] == '\0';
		}

		private async Task<bool> MatchValueWithTrailingSeparatorAsync(string value, CancellationToken cancellationToken)
		{
			bool flag;
			bool flag1;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueAsync(value, cancellationToken).ConfigureAwait(false);
			if (await configuredTaskAwaitable)
			{
				configuredTaskAwaitable = this.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false);
				if (await configuredTaskAwaitable)
				{
					flag1 = (this.IsSeparator(this._chars[this._charPos]) ? true : this._chars[this._charPos] == '\0');
					flag = flag1;
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		private void OnNewLine(int pos)
		{
			this._lineNumber++;
			this._lineStartPos = pos;
		}

		private void ParseComment(bool setToken)
		{
			bool flag;
			this._charPos++;
			if (!this.EnsureChars(1, false))
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			if (this._chars[this._charPos] != '*')
			{
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			this._charPos++;
			int num = this._charPos;
			do
			{
			Label0:
				char chr = this._chars[this._charPos];
				if (chr > '\n')
				{
					if (chr == '\r')
					{
						if (flag)
						{
							this.EndComment(setToken, num, this._charPos);
							return;
						}
						this.ProcessCarriageReturn(true);
						goto Label0;
					}
					else
					{
						if (chr != '*')
						{
							goto Label1;
						}
						this._charPos++;
						if (!flag && this.EnsureChars(0, true) && this._chars[this._charPos] == '/')
						{
							this.EndComment(setToken, num, this._charPos - 1);
							this._charPos++;
							return;
						}
						else
						{
							goto Label0;
						}
					}
				}
				else if (chr == 0)
				{
					if (this._charsUsed == this._charPos)
					{
						continue;
					}
					this._charPos++;
					goto Label0;
				}
				else if (chr == '\n')
				{
					if (flag)
					{
						this.EndComment(setToken, num, this._charPos);
						return;
					}
					this.ProcessLineFeed();
					goto Label0;
				}
			Label1:
				this._charPos++;
				goto Label0;
			}
			while (this.ReadData(true) != 0);
			if (!flag)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			this.EndComment(setToken, num, this._charPos);
		}

		private async Task ParseCommentAsync(bool setToken, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<int> configuredTaskAwaitable;
			bool flag;
			this._charPos++;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable1 = this.EnsureCharsAsync(1, false, cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable1)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			if (this._chars[this._charPos] != '*')
			{
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			this._charPos++;
			int num = this._charPos;
			do
			{
			Label1:
				char chr = this._chars[this._charPos];
				if (chr <= '\n')
				{
					if (chr != 0)
					{
						if (chr == '\n')
						{
							if (flag)
							{
								this.EndComment(setToken, num, this._charPos);
								return;
							}
							else
							{
								this.ProcessLineFeed();
								goto Label1;
							}
						}
					}
					else if (this._charsUsed != this._charPos)
					{
						this._charPos++;
						goto Label1;
					}
					else
					{
						configuredTaskAwaitable = this.ReadDataAsync(true, cancellationToken).ConfigureAwait(false);
						continue;
					}
				}
				else if (chr != '\r')
				{
					if (chr != '*')
					{
						goto Label2;
					}
					this._charPos++;
					if (!flag)
					{
						configuredTaskAwaitable1 = this.EnsureCharsAsync(0, true, cancellationToken).ConfigureAwait(false);
						if (await configuredTaskAwaitable1 && this._chars[this._charPos] == '/')
						{
							this.EndComment(setToken, num, this._charPos - 1);
							this._charPos++;
							return;
						}
						else
						{
							goto Label1;
						}
					}
					else
					{
						goto Label1;
					}
				}
				else if (flag)
				{
					this.EndComment(setToken, num, this._charPos);
					return;
				}
				else
				{
					await this.ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
					goto Label1;
				}
			Label2:
				this._charPos++;
				goto Label1;
			}
			while (await configuredTaskAwaitable != 0);
			if (!flag)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			this.EndComment(setToken, num, this._charPos);
		}

		private void ParseConstructor()
		{
			int num;
			if (!this.MatchValueWithTrailingSeparator("new"))
			{
				throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
			}
			this.EatWhitespace();
			int num1 = this._charPos;
			while (true)
			{
				char chr = this._chars[this._charPos];
				if (chr == 0)
				{
					if (this._charsUsed != this._charPos)
					{
						num = this._charPos;
						this._charPos++;
						break;
					}
					else if (this.ReadData(true) == 0)
					{
						throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
					}
				}
				else if (char.IsLetterOrDigit(chr))
				{
					this._charPos++;
				}
				else if (chr == '\r')
				{
					num = this._charPos;
					this.ProcessCarriageReturn(true);
					break;
				}
				else if (chr == '\n')
				{
					num = this._charPos;
					this.ProcessLineFeed();
					break;
				}
				else if (!char.IsWhiteSpace(chr))
				{
					if (chr != '(')
					{
						throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, chr));
					}
					num = this._charPos;
					break;
				}
				else
				{
					num = this._charPos;
					this._charPos++;
					break;
				}
			}
			this._stringReference = new StringReference(this._chars, num1, num - num1);
			string str = this._stringReference.ToString();
			this.EatWhitespace();
			if (this._chars[this._charPos] != '(')
			{
				throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			this.ClearRecentString();
			base.SetToken(JsonToken.StartConstructor, str);
		}

		private async Task ParseConstructorAsync(CancellationToken cancellationToken)
		{
			int num;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueWithTrailingSeparatorAsync("new", cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
			}
			ConfiguredTaskAwaitable configuredTaskAwaitable1 = this.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable1;
			int num1 = this._charPos;
			while (true)
			{
				char chr = this._chars[this._charPos];
				if (chr == 0)
				{
					if (this._charsUsed != this._charPos)
					{
						num = this._charPos;
						this._charPos++;
						break;
					}
					else
					{
						ConfiguredTaskAwaitable<int> configuredTaskAwaitable2 = this.ReadDataAsync(true, cancellationToken).ConfigureAwait(false);
						if (await configuredTaskAwaitable2 == 0)
						{
							throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
						}
					}
				}
				else if (char.IsLetterOrDigit(chr))
				{
					this._charPos++;
				}
				else if (chr == '\r')
				{
					num = this._charPos;
					configuredTaskAwaitable1 = this.ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable1;
					break;
				}
				else if (chr == '\n')
				{
					num = this._charPos;
					this.ProcessLineFeed();
					break;
				}
				else if (!char.IsWhiteSpace(chr))
				{
					if (chr != '(')
					{
						throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, chr));
					}
					num = this._charPos;
					break;
				}
				else
				{
					num = this._charPos;
					this._charPos++;
					break;
				}
			}
			this._stringReference = new StringReference(this._chars, num1, num - num1);
			string str = this._stringReference.ToString();
			configuredTaskAwaitable1 = this.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable1;
			if (this._chars[this._charPos] != '(')
			{
				throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			this.ClearRecentString();
			base.SetToken(JsonToken.StartConstructor, str);
			str = null;
		}

		private void ParseFalse()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.False))
			{
				throw JsonReaderException.Create(this, "Error parsing boolean value.");
			}
			base.SetToken(JsonToken.Boolean, false);
		}

		private Task ParseFalseAsync(CancellationToken cancellationToken)
		{
			return this.MatchAndSetAsync(JsonConvert.False, JsonToken.Boolean, false, cancellationToken);
		}

		private void ParseNull()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.Null))
			{
				throw JsonReaderException.Create(this, "Error parsing null value.");
			}
			base.SetToken(JsonToken.Null);
		}

		private Task ParseNullAsync(CancellationToken cancellationToken)
		{
			return this.MatchAndSetAsync(JsonConvert.Null, JsonToken.Null, null, cancellationToken);
		}

		private void ParseNumber(ReadType readType)
		{
			this.ShiftBufferIfNeeded();
			char chr = this._chars[this._charPos];
			int num = this._charPos;
			this.ReadNumberIntoBuffer();
			this.ParseReadNumber(readType, chr, num);
		}

		private async Task ParseNumberAsync(ReadType readType, CancellationToken cancellationToken)
		{
			this.ShiftBufferIfNeeded();
			char chr = this._chars[this._charPos];
			int num = this._charPos;
			await this.ReadNumberIntoBufferAsync(cancellationToken).ConfigureAwait(false);
			this.ParseReadNumber(readType, chr, num);
		}

		private object ParseNumberNaN(ReadType readType)
		{
			return this.ParseNumberNaN(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NaN));
		}

		private object ParseNumberNaN(ReadType readType, bool matched)
		{
			if (!matched)
			{
				throw JsonReaderException.Create(this, "Error parsing NaN value.");
			}
			if (readType != ReadType.Read)
			{
				if (readType == ReadType.ReadAsString)
				{
					base.SetToken(JsonToken.String, JsonConvert.NaN);
					return JsonConvert.NaN;
				}
				if (readType != ReadType.ReadAsDouble)
				{
					throw JsonReaderException.Create(this, "Cannot read NaN value.");
				}
			}
			if (this._floatParseHandling == Newtonsoft.Json.FloatParseHandling.Double)
			{
				base.SetToken(JsonToken.Float, double.NaN);
				return double.NaN;
			}
			throw JsonReaderException.Create(this, "Cannot read NaN value.");
		}

		private async Task<object> ParseNumberNaNAsync(ReadType readType, CancellationToken cancellationToken)
		{
			ReadType readType1 = readType;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueWithTrailingSeparatorAsync(JsonConvert.NaN, cancellationToken).ConfigureAwait(false);
			return this.ParseNumberNaN(readType1, await configuredTaskAwaitable);
		}

		private object ParseNumberNegativeInfinity(ReadType readType)
		{
			return this.ParseNumberNegativeInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity));
		}

		private object ParseNumberNegativeInfinity(ReadType readType, bool matched)
		{
			if (!matched)
			{
				throw JsonReaderException.Create(this, "Error parsing -Infinity value.");
			}
			if (readType != ReadType.Read)
			{
				if (readType == ReadType.ReadAsString)
				{
					base.SetToken(JsonToken.String, JsonConvert.NegativeInfinity);
					return JsonConvert.NegativeInfinity;
				}
				if (readType != ReadType.ReadAsDouble)
				{
					throw JsonReaderException.Create(this, "Cannot read -Infinity value.");
				}
			}
			if (this._floatParseHandling == Newtonsoft.Json.FloatParseHandling.Double)
			{
				base.SetToken(JsonToken.Float, double.NegativeInfinity);
				return double.NegativeInfinity;
			}
			throw JsonReaderException.Create(this, "Cannot read -Infinity value.");
		}

		private async Task<object> ParseNumberNegativeInfinityAsync(ReadType readType, CancellationToken cancellationToken)
		{
			ReadType readType1 = readType;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueWithTrailingSeparatorAsync(JsonConvert.NegativeInfinity, cancellationToken).ConfigureAwait(false);
			return this.ParseNumberNegativeInfinity(readType1, await configuredTaskAwaitable);
		}

		private object ParseNumberPositiveInfinity(ReadType readType)
		{
			return this.ParseNumberPositiveInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity));
		}

		private object ParseNumberPositiveInfinity(ReadType readType, bool matched)
		{
			if (!matched)
			{
				throw JsonReaderException.Create(this, "Error parsing Infinity value.");
			}
			if (readType != ReadType.Read)
			{
				if (readType == ReadType.ReadAsString)
				{
					base.SetToken(JsonToken.String, JsonConvert.PositiveInfinity);
					return JsonConvert.PositiveInfinity;
				}
				if (readType != ReadType.ReadAsDouble)
				{
					throw JsonReaderException.Create(this, "Cannot read Infinity value.");
				}
			}
			if (this._floatParseHandling == Newtonsoft.Json.FloatParseHandling.Double)
			{
				base.SetToken(JsonToken.Float, double.PositiveInfinity);
				return double.PositiveInfinity;
			}
			throw JsonReaderException.Create(this, "Cannot read Infinity value.");
		}

		private async Task<object> ParseNumberPositiveInfinityAsync(ReadType readType, CancellationToken cancellationToken)
		{
			ReadType readType1 = readType;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.MatchValueWithTrailingSeparatorAsync(JsonConvert.PositiveInfinity, cancellationToken).ConfigureAwait(false);
			return this.ParseNumberPositiveInfinity(readType1, await configuredTaskAwaitable);
		}

		private bool ParseObject()
		{
			do
			{
			Label1:
				char chr = this._chars[this._charPos];
				if (chr > '\r')
				{
					if (chr == ' ')
					{
						goto Label0;
					}
					if (chr == '/')
					{
						this.ParseComment(true);
						return true;
					}
					if (chr == '}')
					{
						base.SetToken(JsonToken.EndObject);
						this._charPos++;
						return true;
					}
				}
				else if (chr == 0)
				{
					if (this._charsUsed == this._charPos)
					{
						continue;
					}
					this._charPos++;
					goto Label1;
				}
				else
				{
					switch (chr)
					{
						case '\t':
						{
							goto Label0;
						}
						case '\n':
						{
							this.ProcessLineFeed();
							goto Label1;
						}
						case '\r':
						{
							this.ProcessCarriageReturn(false);
							goto Label1;
						}
					}
				}
				if (!char.IsWhiteSpace(chr))
				{
					return this.ParseProperty();
				}
				this._charPos++;
				goto Label1;
			}
			while (this.ReadData(false) != 0);
			return false;
		Label0:
			this._charPos++;
			goto Label1;
		}

		private async Task<bool> ParseObjectAsync(CancellationToken cancellationToken)
		{
			JsonTextReader.<ParseObjectAsync>d__15 variable = new JsonTextReader.<ParseObjectAsync>d__15();
			variable.<>4__this = this;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<ParseObjectAsync>d__15>(ref variable);
			return variable.<>t__builder.Task;
		}

		private bool ParsePostValue(bool ignoreComments)
		{
			do
			{
			Label0:
				char chr = this._chars[this._charPos];
				if (chr <= ')')
				{
					if (chr > '\r')
					{
						if (chr == ' ')
						{
							goto Label2;
						}
						if (chr == ')')
						{
							this._charPos++;
							base.SetToken(JsonToken.EndConstructor);
							return true;
						}
						goto Label1;
					}
					else if (chr == 0)
					{
						if (this._charsUsed == this._charPos)
						{
							continue;
						}
						this._charPos++;
						goto Label0;
					}
					else
					{
						switch (chr)
						{
							case '\t':
							{
								break;
							}
							case '\n':
							{
								this.ProcessLineFeed();
								goto Label0;
							}
							case '\r':
							{
								this.ProcessCarriageReturn(false);
								goto Label0;
							}
							default:
							{
								goto Label1;
							}
						}
					}
				Label2:
					this._charPos++;
					goto Label0;
				}
				else if (chr > '/')
				{
					if (chr == ']')
					{
						this._charPos++;
						base.SetToken(JsonToken.EndArray);
						return true;
					}
					if (chr == '}')
					{
						this._charPos++;
						base.SetToken(JsonToken.EndObject);
						return true;
					}
				}
				else
				{
					if (chr == ',')
					{
						this._charPos++;
						base.SetStateBasedOnCurrent();
						return false;
					}
					if (chr == '/')
					{
						this.ParseComment(!ignoreComments);
						if (!ignoreComments)
						{
							return true;
						}
						else
						{
							goto Label0;
						}
					}
				}
			Label1:
				if (!char.IsWhiteSpace(chr))
				{
					if (!base.SupportMultipleContent || this.Depth != 0)
					{
						throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, chr));
					}
					base.SetStateBasedOnCurrent();
					return false;
				}
				this._charPos++;
				goto Label0;
			}
			while (this.ReadData(false) != 0);
			this._currentState = JsonReader.State.Finished;
			return false;
		}

		private async Task<bool> ParsePostValueAsync(bool ignoreComments, CancellationToken cancellationToken)
		{
			JsonTextReader.<ParsePostValueAsync>d__4 variable = new JsonTextReader.<ParsePostValueAsync>d__4();
			variable.<>4__this = this;
			variable.ignoreComments = ignoreComments;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<ParsePostValueAsync>d__4>(ref variable);
			return variable.<>t__builder.Task;
		}

		private bool ParseProperty()
		{
			char chr;
			string str;
			char chr1 = this._chars[this._charPos];
			if (chr1 != '\"')
			{
				if (chr1 == '\'')
				{
					goto Label2;
				}
				if (!this.ValidIdentifierChar(chr1))
				{
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				chr = '\0';
				this.ShiftBufferIfNeeded();
				this.ParseUnquotedProperty();
				str = (this.PropertyNameTable == null ? this._stringReference.ToString() : this.PropertyNameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) ?? this._stringReference.ToString());
				this.EatWhitespace();
				if (this._chars[this._charPos] != ':')
				{
					throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				this._charPos++;
				base.SetToken(JsonToken.PropertyName, str);
				this._quoteChar = chr;
				this.ClearRecentString();
				return true;
			}
		Label2:
			this._charPos++;
			chr = chr1;
			this.ShiftBufferIfNeeded();
			this.ReadStringIntoBuffer(chr);
			str = (this.PropertyNameTable == null ? this._stringReference.ToString() : this.PropertyNameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) ?? this._stringReference.ToString());
			this.EatWhitespace();
			if (this._chars[this._charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			base.SetToken(JsonToken.PropertyName, str);
			this._quoteChar = chr;
			this.ClearRecentString();
			return true;
		}

		private async Task<bool> ParsePropertyAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable;
			char chr;
			string str;
			char chr1 = this._chars[this._charPos];
			if (chr1 != '\"')
			{
				if (chr1 == '\'')
				{
					goto Label2;
				}
				if (!this.ValidIdentifierChar(chr1))
				{
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				chr = '\0';
				this.ShiftBufferIfNeeded();
				configuredTaskAwaitable = this.ParseUnquotedPropertyAsync(cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				goto Label0;
			}
		Label2:
			this._charPos++;
			chr = chr1;
			this.ShiftBufferIfNeeded();
			configuredTaskAwaitable = this.ReadStringIntoBufferAsync(chr, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		Label0:
			if (this.PropertyNameTable == null)
			{
				str = this._stringReference.ToString();
			}
			else
			{
				string str1 = this.PropertyNameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length);
				if (str1 == null)
				{
					str1 = this._stringReference.ToString();
				}
				str = str1;
			}
			configuredTaskAwaitable = this.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (this._chars[this._charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			base.SetToken(JsonToken.PropertyName, str);
			this._quoteChar = chr;
			this.ClearRecentString();
			return true;
			goto Label2;
		}

		private void ParseReadNumber(ReadType readType, char firstChar, int initialPosition)
		{
			object num;
			JsonToken jsonToken;
			double num1;
			int num2;
			decimal num3;
			double num4;
			long num5;
			decimal num6;
			double num7;
			base.SetPostValueState(true);
			this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
			bool flag = (!char.IsDigit(firstChar) ? false : this._stringReference.Length == 1);
			bool flag1 = (firstChar != '0' || this._stringReference.Length <= 1 || this._stringReference.Chars[this._stringReference.StartIndex + 1] == '.' || this._stringReference.Chars[this._stringReference.StartIndex + 1] == 'e' ? false : this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'E');
			switch (readType)
			{
				case ReadType.Read:
				case ReadType.const_2:
				{
					if (flag)
					{
						num = (long)((ulong)firstChar - 48L);
						jsonToken = JsonToken.Integer;
						break;
					}
					else if (!flag1)
					{
						ParseResult parseResult = ConvertUtils.Int64TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num5);
						if (parseResult == ParseResult.Success)
						{
							num = num5;
							jsonToken = JsonToken.Integer;
							break;
						}
						else if (parseResult != ParseResult.Overflow)
						{
							if (this._floatParseHandling != Newtonsoft.Json.FloatParseHandling.Decimal)
							{
								if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out num7))
								{
									throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
								}
								num = num7;
							}
							else
							{
								parseResult = ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num6);
								if (parseResult != ParseResult.Success)
								{
									throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
								}
								num = num6;
							}
							jsonToken = JsonToken.Float;
							break;
						}
						else
						{
							string str = this._stringReference.ToString();
							if (str.Length > 380)
							{
								throw this.ThrowReaderError("JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
							}
							num = JsonTextReader.BigIntegerParse(str, CultureInfo.InvariantCulture);
							jsonToken = JsonToken.Integer;
							break;
						}
					}
					else
					{
						string str1 = this._stringReference.ToString();
						try
						{
							num = (str1.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str1, 16) : Convert.ToInt64(str1, 8));
						}
						catch (Exception exception1)
						{
							Exception exception = exception1;
							throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, str1), exception);
						}
						jsonToken = JsonToken.Integer;
						break;
					}
				}
				case ReadType.const_1:
				{
					if (flag)
					{
						num = firstChar - 48;
					}
					else if (!flag1)
					{
						ParseResult parseResult1 = ConvertUtils.Int32TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num2);
						if (parseResult1 != ParseResult.Success)
						{
							if (parseResult1 != ParseResult.Overflow)
							{
								throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
							}
							throw this.ThrowReaderError("JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						num = num2;
					}
					else
					{
						string str2 = this._stringReference.ToString();
						try
						{
							num = (str2.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(str2, 16) : Convert.ToInt32(str2, 8));
						}
						catch (Exception exception3)
						{
							Exception exception2 = exception3;
							throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, str2), exception2);
						}
					}
					jsonToken = JsonToken.Integer;
					break;
				}
				case ReadType.ReadAsBytes:
				case ReadType.ReadAsDateTime:
				case ReadType.ReadAsDateTimeOffset:
				{
					throw JsonReaderException.Create(this, "Cannot read number value as type.");
				}
				case ReadType.ReadAsString:
				{
					string str3 = this._stringReference.ToString();
					if (flag1)
					{
						try
						{
							if (!str3.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
							{
								Convert.ToInt64(str3, 8);
							}
							else
							{
								Convert.ToInt64(str3, 16);
							}
						}
						catch (Exception exception5)
						{
							Exception exception4 = exception5;
							throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, str3), exception4);
						}
					}
					else if (!double.TryParse(str3, NumberStyles.Float, CultureInfo.InvariantCulture, out num1))
					{
						throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
					}
					jsonToken = JsonToken.String;
					num = str3;
					break;
				}
				case ReadType.ReadAsDecimal:
				{
					if (flag)
					{
						num = firstChar - new decimal(48);
					}
					else if (!flag1)
					{
						if (ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num3) != ParseResult.Success)
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						num = num3;
					}
					else
					{
						string str4 = this._stringReference.ToString();
						try
						{
							num = Convert.ToDecimal((str4.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str4, 16) : Convert.ToInt64(str4, 8)));
						}
						catch (Exception exception7)
						{
							Exception exception6 = exception7;
							throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, str4), exception6);
						}
					}
					jsonToken = JsonToken.Float;
					break;
				}
				case ReadType.ReadAsDouble:
				{
					if (flag)
					{
						num = (double)firstChar - 48;
					}
					else if (!flag1)
					{
						if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out num4))
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						num = num4;
					}
					else
					{
						string str5 = this._stringReference.ToString();
						try
						{
							num = Convert.ToDouble((str5.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str5, 16) : Convert.ToInt64(str5, 8)));
						}
						catch (Exception exception9)
						{
							Exception exception8 = exception9;
							throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, str5), exception8);
						}
					}
					jsonToken = JsonToken.Float;
					break;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Cannot read number value as type.");
				}
			}
			this.ClearRecentString();
			base.SetToken(jsonToken, num, false);
		}

		private void ParseReadString(char quote, ReadType readType)
		{
			Guid guid;
			byte[] numArray;
			Newtonsoft.Json.DateParseHandling dateParseHandling;
			DateTime dateTime;
			DateTimeOffset dateTimeOffset;
			base.SetPostValueState(true);
			switch (readType)
			{
				case ReadType.const_1:
				case ReadType.ReadAsDecimal:
				case ReadType.ReadAsBoolean:
				{
					return;
				}
				case ReadType.const_2:
				case ReadType.ReadAsDateTime:
				case ReadType.ReadAsDateTimeOffset:
				case ReadType.ReadAsDouble:
				{
					if (this._dateParseHandling != Newtonsoft.Json.DateParseHandling.None)
					{
						if (readType != ReadType.ReadAsDateTime)
						{
							dateParseHandling = (readType != ReadType.ReadAsDateTimeOffset ? this._dateParseHandling : Newtonsoft.Json.DateParseHandling.DateTimeOffset);
						}
						else
						{
							dateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
						}
						if (dateParseHandling == Newtonsoft.Json.DateParseHandling.DateTime)
						{
							if (DateTimeUtils.TryParseDateTime(this._stringReference, base.DateTimeZoneHandling, base.DateFormatString, base.Culture, out dateTime))
							{
								base.SetToken(JsonToken.Date, dateTime, false);
								return;
							}
						}
						else if (DateTimeUtils.TryParseDateTimeOffset(this._stringReference, base.DateFormatString, base.Culture, out dateTimeOffset))
						{
							base.SetToken(JsonToken.Date, dateTimeOffset, false);
							return;
						}
					}
					base.SetToken(JsonToken.String, this._stringReference.ToString(), false);
					this._quoteChar = quote;
					return;
				}
				case ReadType.ReadAsBytes:
				{
					if (this._stringReference.Length != 0)
					{
						numArray = (this._stringReference.Length != 36 || !ConvertUtils.TryConvertGuid(this._stringReference.ToString(), out guid) ? Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) : guid.ToByteArray());
					}
					else
					{
						numArray = CollectionUtils.ArrayEmpty<byte>();
					}
					base.SetToken(JsonToken.Bytes, numArray, false);
					return;
				}
				case ReadType.ReadAsString:
				{
					base.SetToken(JsonToken.String, this._stringReference.ToString(), false);
					this._quoteChar = quote;
					return;
				}
				default:
				{
					goto case ReadType.ReadAsDouble;
				}
			}
		}

		private void ParseString(char quote, ReadType readType)
		{
			this._charPos++;
			this.ShiftBufferIfNeeded();
			this.ReadStringIntoBuffer(quote);
			this.ParseReadString(quote, readType);
		}

		private async Task ParseStringAsync(char quote, ReadType readType, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			this._charPos++;
			this.ShiftBufferIfNeeded();
			await this.ReadStringIntoBufferAsync(quote, cancellationToken).ConfigureAwait(false);
			this.ParseReadString(quote, readType);
		}

		private void ParseTrue()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.True))
			{
				throw JsonReaderException.Create(this, "Error parsing boolean value.");
			}
			base.SetToken(JsonToken.Boolean, true);
		}

		private Task ParseTrueAsync(CancellationToken cancellationToken)
		{
			return this.MatchAndSetAsync(JsonConvert.True, JsonToken.Boolean, true, cancellationToken);
		}

		private void ParseUndefined()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.Undefined))
			{
				throw JsonReaderException.Create(this, "Error parsing undefined value.");
			}
			base.SetToken(JsonToken.Undefined);
		}

		private Task ParseUndefinedAsync(CancellationToken cancellationToken)
		{
			return this.MatchAndSetAsync(JsonConvert.Undefined, JsonToken.Undefined, null, cancellationToken);
		}

		private char ParseUnicode()
		{
			return this.ConvertUnicode(this.EnsureChars(4, true));
		}

		private async Task<char> ParseUnicodeAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.EnsureCharsAsync(4, true, cancellationToken).ConfigureAwait(false);
			return this.ConvertUnicode(await configuredTaskAwaitable);
		}

		private void ParseUnquotedProperty()
		{
			int num = this._charPos;
			do
			{
			Label0:
				char chr = this._chars[this._charPos];
				if (chr == 0)
				{
					if (this._charsUsed == this._charPos)
					{
						continue;
					}
					this._stringReference = new StringReference(this._chars, num, this._charPos - num);
					return;
				}
				else if (this.ReadUnquotedPropertyReportIfDone(chr, num))
				{
					return;
				}
				else
				{
					goto Label0;
				}
			}
			while (this.ReadData(true) != 0);
			throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
		}

		private async Task ParseUnquotedPropertyAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<int> configuredTaskAwaitable;
			int num = this._charPos;
			do
			{
			Label1:
				char chr = this._chars[this._charPos];
				if (chr == 0)
				{
					if (this._charsUsed != this._charPos)
					{
						this._stringReference = new StringReference(this._chars, num, this._charPos - num);
						return;
					}
					else
					{
						configuredTaskAwaitable = this.ReadDataAsync(true, cancellationToken).ConfigureAwait(false);
					}
				}
				else if (this.ReadUnquotedPropertyReportIfDone(chr, num))
				{
					return;
				}
				else
				{
					goto Label1;
				}
			}
			while (await configuredTaskAwaitable != 0);
			throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
		}

		private bool ParseValue()
		{
			char chr;
			while (true)
			{
				chr = this._chars[this._charPos];
				if (chr > 'N')
				{
					if (chr <= 'f')
					{
						if (chr == '[')
						{
							this._charPos++;
							base.SetToken(JsonToken.StartArray);
							return true;
						}
						if (chr == ']')
						{
							this._charPos++;
							base.SetToken(JsonToken.EndArray);
							return true;
						}
						if (chr == 'f')
						{
							this.ParseFalse();
							return true;
						}
					}
					else if (chr > 't')
					{
						if (chr == 'u')
						{
							this.ParseUndefined();
							return true;
						}
						if (chr == '{')
						{
							this._charPos++;
							base.SetToken(JsonToken.StartObject);
							return true;
						}
					}
					else
					{
						if (chr == 'n')
						{
							if (!this.EnsureChars(1, true))
							{
								this._charPos++;
								throw base.CreateUnexpectedEndException();
							}
							char chr1 = this._chars[this._charPos + 1];
							if (chr1 != 'u')
							{
								if (chr1 != 'e')
								{
									throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
								}
								this.ParseConstructor();
							}
							else
							{
								this.ParseNull();
							}
							return true;
						}
						if (chr == 't')
						{
							this.ParseTrue();
							return true;
						}
					}
				}
				else if (chr > ' ')
				{
					if (chr > '/')
					{
						if (chr == 'I')
						{
							this.ParseNumberPositiveInfinity(ReadType.Read);
							return true;
						}
						if (chr == 'N')
						{
							this.ParseNumberNaN(ReadType.Read);
							return true;
						}
					}
					else
					{
						if (chr == '\"')
						{
							break;
						}
						switch (chr)
						{
							case '\'':
							{
								this.ParseString(chr, ReadType.Read);
								return true;
							}
							case ')':
							{
								this._charPos++;
								base.SetToken(JsonToken.EndConstructor);
								return true;
							}
							case ',':
							{
								base.SetToken(JsonToken.Undefined);
								return true;
							}
							case '-':
							{
								if (!this.EnsureChars(1, true) || this._chars[this._charPos + 1] != 'I')
								{
									this.ParseNumber(ReadType.Read);
								}
								else
								{
									this.ParseNumberNegativeInfinity(ReadType.Read);
								}
								return true;
							}
							case '/':
							{
								this.ParseComment(true);
								return true;
							}
						}
					}
				}
				else if (chr != 0)
				{
					switch (chr)
					{
						case '\t':
						{
							this._charPos++;
							continue;
						}
						case '\n':
						{
							this.ProcessLineFeed();
							continue;
						}
						case '\v':
						case '\f':
						{
							break;
						}
						case '\r':
						{
							this.ProcessCarriageReturn(false);
							continue;
						}
						default:
						{
							if (chr == ' ')
							{
								goto case '\t';
							}
							break;
						}
					}
				}
				else if (this._charsUsed != this._charPos)
				{
					this._charPos++;
					continue;
				}
				else if (this.ReadData(false) == 0)
				{
					return false;
				}
				if (!char.IsWhiteSpace(chr))
				{
					if (!char.IsNumber(chr) && chr != '-')
					{
						if (chr != '.')
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
					}
					this.ParseNumber(ReadType.Read);
					return true;
				}
				this._charPos++;
			}
			this.ParseString(chr, ReadType.Read);
			return true;
		}

		private async Task<bool> ParseValueAsync(CancellationToken cancellationToken)
		{
			bool flag;
			char chr;
			ConfiguredTaskAwaitable configuredTaskAwaitable;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable1;
			ConfiguredTaskAwaitable<object> configuredTaskAwaitable2;
			while (true)
			{
				chr = this._chars[this._charPos];
				if (chr <= 'N')
				{
					if (chr <= ' ')
					{
						if (chr != 0)
						{
							switch (chr)
							{
								case '\t':
								{
									this._charPos++;
									continue;
								}
								case '\n':
								{
									this.ProcessLineFeed();
									continue;
								}
								case '\v':
								case '\f':
								{
									break;
								}
								case '\r':
								{
									configuredTaskAwaitable = this.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
									await configuredTaskAwaitable;
									continue;
								}
								default:
								{
									if (chr == ' ')
									{
										goto case '\t';
									}
									break;
								}
							}
						}
						else if (this._charsUsed != this._charPos)
						{
							this._charPos++;
							continue;
						}
						else
						{
							ConfiguredTaskAwaitable<int> configuredTaskAwaitable3 = this.ReadDataAsync(false, cancellationToken).ConfigureAwait(false);
							if (await configuredTaskAwaitable3 == 0)
							{
								flag = false;
								return flag;
							}
						}
					}
					else if (chr <= '/')
					{
						if (chr == '\"')
						{
							break;
						}
						switch (chr)
						{
							case '\'':
							{
								configuredTaskAwaitable = this.ParseStringAsync(chr, ReadType.Read, cancellationToken).ConfigureAwait(false);
								await configuredTaskAwaitable;
								flag = true;
								return flag;
							}
							case ')':
							{
								this._charPos++;
								base.SetToken(JsonToken.EndConstructor);
								flag = true;
								return flag;
							}
							case ',':
							{
								base.SetToken(JsonToken.Undefined);
								flag = true;
								return flag;
							}
							case '-':
							{
								configuredTaskAwaitable1 = this.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false);
								if (!await configuredTaskAwaitable1 || this._chars[this._charPos + 1] != 'I')
								{
									configuredTaskAwaitable = this.ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
									await configuredTaskAwaitable;
								}
								else
								{
									configuredTaskAwaitable2 = this.ParseNumberNegativeInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
									await configuredTaskAwaitable2;
								}
								flag = true;
								return flag;
							}
							case '/':
							{
								configuredTaskAwaitable = this.ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
								await configuredTaskAwaitable;
								flag = true;
								return flag;
							}
						}
					}
					else if (chr == 'I')
					{
						configuredTaskAwaitable2 = this.ParseNumberPositiveInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable2;
						flag = true;
						return flag;
					}
					else if (chr == 'N')
					{
						configuredTaskAwaitable2 = this.ParseNumberNaNAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable2;
						flag = true;
						return flag;
					}
				}
				else if (chr <= 'f')
				{
					if (chr == '[')
					{
						this._charPos++;
						base.SetToken(JsonToken.StartArray);
						flag = true;
						return flag;
					}
					else if (chr == ']')
					{
						this._charPos++;
						base.SetToken(JsonToken.EndArray);
						flag = true;
						return flag;
					}
					else if (chr == 'f')
					{
						configuredTaskAwaitable = this.ParseFalseAsync(cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable;
						flag = true;
						return flag;
					}
				}
				else if (chr <= 't')
				{
					if (chr == 'n')
					{
						configuredTaskAwaitable1 = this.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false);
						if (!await configuredTaskAwaitable1)
						{
							this._charPos++;
							throw base.CreateUnexpectedEndException();
						}
						char chr1 = this._chars[this._charPos + 1];
						if (chr1 == 'e')
						{
							configuredTaskAwaitable = this.ParseConstructorAsync(cancellationToken).ConfigureAwait(false);
							await configuredTaskAwaitable;
						}
						else
						{
							if (chr1 != 'u')
							{
								throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
							}
							configuredTaskAwaitable = this.ParseNullAsync(cancellationToken).ConfigureAwait(false);
							await configuredTaskAwaitable;
						}
						flag = true;
						return flag;
					}
					else if (chr == 't')
					{
						configuredTaskAwaitable = this.ParseTrueAsync(cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable;
						flag = true;
						return flag;
					}
				}
				else if (chr == 'u')
				{
					configuredTaskAwaitable = this.ParseUndefinedAsync(cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
					flag = true;
					return flag;
				}
				else if (chr == '{')
				{
					this._charPos++;
					base.SetToken(JsonToken.StartObject);
					flag = true;
					return flag;
				}
				if (!char.IsWhiteSpace(chr))
				{
					if (!char.IsNumber(chr) && chr != '-')
					{
						if (chr != '.')
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
					}
					configuredTaskAwaitable = this.ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
					flag = true;
					return flag;
				}
				else
				{
					this._charPos++;
				}
			}
			configuredTaskAwaitable = this.ParseStringAsync(chr, ReadType.Read, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			flag = true;
			return flag;
		}

		private void PrepareBufferForReadData(bool append, int charsRequired)
		{
			if (this._charsUsed + charsRequired >= (int)this._chars.Length - 1)
			{
				if (append)
				{
					int length = (int)this._chars.Length * 2;
					int num = Math.Max((length < 0 ? 2147483647 : length), this._charsUsed + charsRequired + 1);
					char[] chrArray = BufferUtils.RentBuffer(this._arrayPool, num);
					JsonTextReader.BlockCopyChars(this._chars, 0, chrArray, 0, (int)this._chars.Length);
					BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
					this._chars = chrArray;
					return;
				}
				int num1 = this._charsUsed - this._charPos;
				if (num1 + charsRequired + 1 >= (int)this._chars.Length)
				{
					char[] chrArray1 = BufferUtils.RentBuffer(this._arrayPool, num1 + charsRequired + 1);
					if (num1 > 0)
					{
						JsonTextReader.BlockCopyChars(this._chars, this._charPos, chrArray1, 0, num1);
					}
					BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
					this._chars = chrArray1;
				}
				else if (num1 > 0)
				{
					JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num1);
				}
				this._lineStartPos -= this._charPos;
				this._charPos = 0;
				this._charsUsed = num1;
			}
		}

		private void ProcessCarriageReturn(bool append)
		{
			this._charPos++;
			this.SetNewLine(this.EnsureChars(1, append));
		}

		private Task ProcessCarriageReturnAsync(bool append, CancellationToken cancellationToken)
		{
			this._charPos++;
			Task<bool> task = this.EnsureCharsAsync(1, append, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return this.ProcessCarriageReturnAsync(task);
			}
			this.SetNewLine(task.Result);
			return AsyncUtils.CompletedTask;
		}

		private async Task ProcessCarriageReturnAsync(Task<bool> task)
		{
			this.SetNewLine(await task.ConfigureAwait(false));
		}

		private void ProcessLineFeed()
		{
			this._charPos++;
			this.OnNewLine(this._charPos);
		}

		private void ProcessValueComma()
		{
			this._charPos++;
			if (this._currentState != JsonReader.State.PostValue)
			{
				base.SetToken(JsonToken.Undefined);
				this._charPos--;
				throw this.CreateUnexpectedCharacterException(',');
			}
			base.SetStateBasedOnCurrent();
		}

		public override bool Read()
		{
			this.EnsureBuffer();
			do
			{
				switch (this._currentState)
				{
					case JsonReader.State.Start:
					case JsonReader.State.Property:
					case JsonReader.State.ArrayStart:
					case JsonReader.State.Array:
					case JsonReader.State.ConstructorStart:
					case JsonReader.State.Constructor:
					{
						return this.ParseValue();
					}
					case JsonReader.State.Complete:
					case JsonReader.State.Closed:
					case JsonReader.State.Error:
					{
						throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
					}
					case JsonReader.State.ObjectStart:
					case JsonReader.State.Object:
					{
						return this.ParseObject();
					}
					case JsonReader.State.PostValue:
					{
						continue;
					}
					case JsonReader.State.Finished:
					{
						if (!this.EnsureChars(0, false))
						{
							base.SetToken(JsonToken.None);
							return false;
						}
						this.EatWhitespace();
						if (this._isEndOfFile)
						{
							base.SetToken(JsonToken.None);
							return false;
						}
						if (this._chars[this._charPos] != '/')
						{
							throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
						}
						this.ParseComment(true);
						return true;
					}
					default:
					{
						throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
					}
				}
			}
			while (!this.ParsePostValue(false));
			return true;
		}

		public override bool? ReadAsBoolean()
		{
			bool? nullable;
			char chr;
			bool flag;
			this.EnsureBuffer();
			switch (this._currentState)
			{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
				{
					while (true)
					{
						chr = this._chars[this._charPos];
						if (chr > '9')
						{
							if (chr > 'f')
							{
								if (chr == 'n')
								{
									this.HandleNull();
									nullable = null;
									return nullable;
								}
								if (chr == 't')
								{
									break;
								}
							}
							else
							{
								if (chr == ']')
								{
									this._charPos++;
									if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
									{
										if (this._currentState != JsonReader.State.PostValue)
										{
											throw this.CreateUnexpectedCharacterException(chr);
										}
									}
									base.SetToken(JsonToken.EndArray);
									nullable = null;
									return nullable;
								}
								if (chr == 'f')
								{
									break;
								}
							}
						}
						else if (chr != 0)
						{
							switch (chr)
							{
								case '\t':
								{
								Label0:
									this._charPos++;
									continue;
								}
								case '\n':
								{
									this.ProcessLineFeed();
									continue;
								}
								case '\v':
								case '\f':
								{
									break;
								}
								case '\r':
								{
									this.ProcessCarriageReturn(false);
									continue;
								}
								default:
								{
									switch (chr)
									{
										case ' ':
										{
											goto Label0;
										}
										case '\"':
										case '\'':
										{
											this.ParseString(chr, ReadType.Read);
											return base.ReadBooleanString(this._stringReference.ToString());
										}
										case ',':
										{
											this.ProcessValueComma();
											continue;
										}
										case '-':
										case '.':
										case '0':
										case '1':
										case '2':
										case '3':
										case '4':
										case '5':
										case '6':
										case '7':
										case '8':
										case '9':
										{
											this.ParseNumber(ReadType.Read);
											object value = this.Value;
											object obj = value;
											flag = (!(value is BigInteger) ? Convert.ToBoolean(this.Value, CultureInfo.InvariantCulture) : (BigInteger)obj != 0L);
											base.SetToken(JsonToken.Boolean, flag, false);
											return new bool?(flag);
										}
										case '/':
										{
											this.ParseComment(false);
											continue;
										}
									}
									break;
								}
							}
						}
						else if (this.ReadNullChar())
						{
							base.SetToken(JsonToken.None, null, false);
							nullable = null;
							return nullable;
						}
						this._charPos++;
						if (!char.IsWhiteSpace(chr))
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
					}
					bool flag1 = chr == 't';
					bool flag2 = flag1;
					if (!this.MatchValueWithTrailingSeparator((flag1 ? JsonConvert.True : JsonConvert.False)))
					{
						throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
					}
					base.SetToken(JsonToken.Boolean, flag2);
					return new bool?(flag2);
				}
				case JsonReader.State.Complete:
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
				case JsonReader.State.PostValue:
				{
					if (!this.ParsePostValue(true))
					{
						goto case JsonReader.State.Constructor;
					}
					nullable = null;
					return nullable;
				}
				case JsonReader.State.Finished:
				{
					this.ReadFinished();
					nullable = null;
					return nullable;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
		}

		public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsBooleanAsync(cancellationToken);
			}
			return this.DoReadAsBooleanAsync(cancellationToken);
		}

		public override byte[] ReadAsBytes()
		{
			this.EnsureBuffer();
			bool flag = false;
			switch (this._currentState)
			{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
				{
					do
					{
					Label0:
						char chr = this._chars[this._charPos];
						if (chr <= '\'')
						{
							if (chr > '\r')
							{
								if (chr == ' ')
								{
									goto Label3;
								}
								if (chr == '\"' || chr == '\'')
								{
									this.ParseString(chr, ReadType.ReadAsBytes);
									byte[] value = (byte[])this.Value;
									if (flag)
									{
										base.ReaderReadAndAssert();
										if (this.TokenType != JsonToken.EndObject)
										{
											throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
										}
										base.SetToken(JsonToken.Bytes, value, false);
									}
									return value;
								}
								goto Label2;
							}
							else
							{
								if (chr == 0)
								{
									continue;
								}
								switch (chr)
								{
									case '\t':
									{
										break;
									}
									case '\n':
									{
										this.ProcessLineFeed();
										goto Label0;
									}
									case '\r':
									{
										this.ProcessCarriageReturn(false);
										goto Label0;
									}
									default:
									{
										goto Label2;
									}
								}
							}
						Label3:
							this._charPos++;
							goto Label0;
						}
						else if (chr > '[')
						{
							if (chr == ']')
							{
								this._charPos++;
								if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
								{
									if (this._currentState != JsonReader.State.PostValue)
									{
										throw this.CreateUnexpectedCharacterException(chr);
									}
								}
								base.SetToken(JsonToken.EndArray);
								return null;
							}
							if (chr == 'n')
							{
								this.HandleNull();
								return null;
							}
							if (chr != '{')
							{
								goto Label2;
							}
							this._charPos++;
							base.SetToken(JsonToken.StartObject);
							base.ReadIntoWrappedTypeObject();
							flag = true;
							goto Label0;
						}
						else if (chr == ',')
						{
							this.ProcessValueComma();
							goto Label0;
						}
						else if (chr == '/')
						{
							this.ParseComment(false);
							goto Label0;
						}
						else if (chr == '[')
						{
							this._charPos++;
							base.SetToken(JsonToken.StartArray);
							return base.ReadArrayIntoByteArray();
						}
					Label2:
						this._charPos++;
						if (!char.IsWhiteSpace(chr))
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
						else
						{
							goto Label0;
						}
					}
					while (!this.ReadNullChar());
					base.SetToken(JsonToken.None, null, false);
					return null;
				}
				case JsonReader.State.Complete:
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
				case JsonReader.State.PostValue:
				{
					if (!this.ParsePostValue(true))
					{
						goto case JsonReader.State.Constructor;
					}
					return null;
				}
				case JsonReader.State.Finished:
				{
					this.ReadFinished();
					return null;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
		}

		public override Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsBytesAsync(cancellationToken);
			}
			return this.DoReadAsBytesAsync(cancellationToken);
		}

		public override DateTime? ReadAsDateTime()
		{
			return (DateTime?)this.ReadStringValue(ReadType.ReadAsDateTime);
		}

		public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsDateTimeAsync(cancellationToken);
			}
			return this.DoReadAsDateTimeAsync(cancellationToken);
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return (DateTimeOffset?)this.ReadStringValue(ReadType.ReadAsDateTimeOffset);
		}

		public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsDateTimeOffsetAsync(cancellationToken);
			}
			return this.DoReadAsDateTimeOffsetAsync(cancellationToken);
		}

		public override decimal? ReadAsDecimal()
		{
			return (decimal?)this.ReadNumberValue(ReadType.ReadAsDecimal);
		}

		public override Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsDecimalAsync(cancellationToken);
			}
			return this.DoReadAsDecimalAsync(cancellationToken);
		}

		public override double? ReadAsDouble()
		{
			return (double?)this.ReadNumberValue(ReadType.ReadAsDouble);
		}

		public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsDoubleAsync(cancellationToken);
			}
			return this.DoReadAsDoubleAsync(cancellationToken);
		}

		public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsInt32Async(cancellationToken);
			}
			return this.DoReadAsInt32Async(cancellationToken);
		}

		public override string ReadAsString()
		{
			return (string)this.ReadStringValue(ReadType.ReadAsString);
		}

		public override Task<string> ReadAsStringAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsStringAsync(cancellationToken);
			}
			return this.DoReadAsStringAsync(cancellationToken);
		}

		public override Task<bool> ReadAsync(CancellationToken cancellationToken = null)
		{
			if (!this._safeAsync)
			{
				return base.ReadAsync(cancellationToken);
			}
			return this.DoReadAsync(cancellationToken);
		}

		private bool ReadChars(int relativePosition, bool append)
		{
			if (this._isEndOfFile)
			{
				return false;
			}
			int num = this._charPos + relativePosition - this._charsUsed + 1;
			int num1 = 0;
			do
			{
				int num2 = this.ReadData(append, num - num1);
				if (num2 == 0)
				{
					break;
				}
				num1 += num2;
			}
			while (num1 < num);
			if (num1 < num)
			{
				return false;
			}
			return true;
		}

		private async Task<bool> ReadCharsAsync(int relativePosition, bool append, CancellationToken cancellationToken)
		{
			bool flag;
			int num = this._charPos + relativePosition - this._charsUsed + 1;
			do
			{
				ConfiguredTaskAwaitable<int> configuredTaskAwaitable = this.ReadDataAsync(append, num, cancellationToken).ConfigureAwait(false);
				int num1 = await configuredTaskAwaitable;
				if (num1 != 0)
				{
					num -= num1;
				}
				else
				{
					flag = false;
					return flag;
				}
			}
			while (num > 0);
			flag = true;
			return flag;
		}

		private int ReadData(bool append)
		{
			return this.ReadData(append, 0);
		}

		private int ReadData(bool append, int charsRequired)
		{
			if (this._isEndOfFile)
			{
				return 0;
			}
			this.PrepareBufferForReadData(append, charsRequired);
			int length = (int)this._chars.Length - this._charsUsed - 1;
			int num = this._reader.Read(this._chars, this._charsUsed, length);
			this._charsUsed += num;
			if (num == 0)
			{
				this._isEndOfFile = true;
			}
			this._chars[this._charsUsed] = '\0';
			return num;
		}

		private Task<int> ReadDataAsync(bool append, CancellationToken cancellationToken)
		{
			return this.ReadDataAsync(append, 0, cancellationToken);
		}

		private async Task<int> ReadDataAsync(bool append, int charsRequired, CancellationToken cancellationToken)
		{
			int num;
			if (!this._isEndOfFile)
			{
				this.PrepareBufferForReadData(append, charsRequired);
				ConfiguredTaskAwaitable<int> configuredTaskAwaitable = this._reader.ReadAsync(this._chars, this._charsUsed, (int)this._chars.Length - this._charsUsed - 1, cancellationToken).ConfigureAwait(false);
				int num1 = await configuredTaskAwaitable;
				this._charsUsed += num1;
				if (num1 == 0)
				{
					this._isEndOfFile = true;
				}
				this._chars[this._charsUsed] = '\0';
				num = num1;
			}
			else
			{
				num = 0;
			}
			return num;
		}

		private void ReadFinished()
		{
			if (this.EnsureChars(0, false))
			{
				this.EatWhitespace();
				if (this._isEndOfFile)
				{
					return;
				}
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				this.ParseComment(false);
			}
			base.SetToken(JsonToken.None);
		}

		private async Task ReadFinishedAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false);
			if (await configuredTaskAwaitable)
			{
				ConfiguredTaskAwaitable configuredTaskAwaitable1 = this.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable1;
				if (!this._isEndOfFile)
				{
					if (this._chars[this._charPos] != '/')
					{
						throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
					}
					configuredTaskAwaitable1 = this.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable1;
				}
				else
				{
					base.SetToken(JsonToken.None);
					return;
				}
			}
			base.SetToken(JsonToken.None);
		}

		private async Task<bool> ReadFromFinishedAsync(CancellationToken cancellationToken)
		{
			bool flag;
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false);
			if (!await configuredTaskAwaitable)
			{
				base.SetToken(JsonToken.None);
				flag = false;
			}
			else
			{
				ConfiguredTaskAwaitable configuredTaskAwaitable1 = this.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable1;
				if (!this._isEndOfFile)
				{
					if (this._chars[this._charPos] != '/')
					{
						throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
					}
					configuredTaskAwaitable1 = this.ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable1;
					flag = true;
				}
				else
				{
					base.SetToken(JsonToken.None);
					flag = false;
				}
			}
			return flag;
		}

		private async Task ReadIntoWrappedTypeObjectAsync(CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = base.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (this.Value != null && this.Value.ToString() == "$type")
			{
				configuredTaskAwaitable = base.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
				{
					configuredTaskAwaitable = base.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
					if (this.Value.ToString() == "$value")
					{
						return;
					}
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
		}

		private bool ReadNullChar()
		{
			if (this._charsUsed != this._charPos)
			{
				this._charPos++;
			}
			else if (this.ReadData(false) == 0)
			{
				this._isEndOfFile = true;
				return true;
			}
			return false;
		}

		private async Task<bool> ReadNullCharAsync(CancellationToken cancellationToken)
		{
			bool flag;
			if (this._charsUsed != this._charPos)
			{
				this._charPos++;
			}
			else
			{
				ConfiguredTaskAwaitable<int> configuredTaskAwaitable = this.ReadDataAsync(false, cancellationToken).ConfigureAwait(false);
				if (await configuredTaskAwaitable == 0)
				{
					this._isEndOfFile = true;
					flag = true;
					return flag;
				}
			}
			flag = false;
			return flag;
		}

		private bool ReadNumberCharIntoBuffer(char currentChar, int charPos)
		{
			if (currentChar > 'X')
			{
				switch (currentChar)
				{
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					{
						break;
					}
					default:
					{
						if (currentChar != 'x')
						{
							this._charPos = charPos;
							if (!char.IsWhiteSpace(currentChar) && currentChar != ',' && currentChar != '}' && currentChar != ']' && currentChar != ')')
							{
								if (currentChar != '/')
								{
									throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
								}
							}
							return true;
						}
						break;
					}
				}
			}
			else
			{
				switch (currentChar)
				{
					case '+':
					case '-':
					case '.':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					{
						break;
					}
					case ',':
					case '/':
					case ':':
					case ';':
					case '<':
					case '=':
					case '>':
					case '?':
					case '@':
					{
						this._charPos = charPos;
						if (!char.IsWhiteSpace(currentChar) && currentChar != ',' && currentChar != '}' && currentChar != ']' && currentChar != ')')
						{
							if (currentChar != '/')
							{
								throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
							}
						}
						return true;
					}
					default:
					{
						if (currentChar == 'X')
						{
							break;
						}
						this._charPos = charPos;
						if (!char.IsWhiteSpace(currentChar) && currentChar != ',' && currentChar != '}' && currentChar != ']' && currentChar != ')')
						{
							if (currentChar != '/')
							{
								throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		private void ReadNumberIntoBuffer()
		{
			int num = this._charPos;
			do
			{
			Label0:
				char chr = this._chars[num];
				if (chr != 0)
				{
					if (this.ReadNumberCharIntoBuffer(chr, num))
					{
						return;
					}
					num++;
					goto Label0;
				}
				else
				{
					this._charPos = num;
					if (this._charsUsed == num)
					{
						continue;
					}
					return;
				}
			}
			while (this.ReadData(true) != 0);
		}

		private async Task ReadNumberIntoBufferAsync(CancellationToken cancellationToken)
		{
			int num = this._charPos;
			while (true)
			{
				char chr = this._chars[num];
				if (chr == 0)
				{
					this._charPos = num;
					if (this._charsUsed != num)
					{
						break;
					}
					ConfiguredTaskAwaitable<int> configuredTaskAwaitable = this.ReadDataAsync(true, cancellationToken).ConfigureAwait(false);
					if (await configuredTaskAwaitable == 0)
					{
						break;
					}
				}
				else if (this.ReadNumberCharIntoBuffer(chr, num))
				{
					return;
				}
				else
				{
					num++;
				}
			}
		}

		private object ReadNumberValue(ReadType readType)
		{
			this.EnsureBuffer();
			switch (this._currentState)
			{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
				{
					do
					{
					Label0:
						char chr = this._chars[this._charPos];
						if (chr <= '9')
						{
							if (chr == 0)
							{
								continue;
							}
							switch (chr)
							{
								case '\t':
								{
								Label2:
									this._charPos++;
									goto Label0;
								}
								case '\n':
								{
									this.ProcessLineFeed();
									goto Label0;
								}
								case '\v':
								case '\f':
								{
									break;
								}
								case '\r':
								{
									this.ProcessCarriageReturn(false);
									goto Label0;
								}
								default:
								{
									switch (chr)
									{
										case ' ':
										{
											goto Label2;
										}
										case '\"':
										case '\'':
										{
											this.ParseString(chr, readType);
											return this.FinishReadQuotedNumber(readType);
										}
										case ',':
										{
											this.ProcessValueComma();
											goto Label0;
										}
										case '-':
										{
											if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
											{
												return this.ParseNumberNegativeInfinity(readType);
											}
											this.ParseNumber(readType);
											return this.Value;
										}
										case '.':
										case '0':
										case '1':
										case '2':
										case '3':
										case '4':
										case '5':
										case '6':
										case '7':
										case '8':
										case '9':
										{
											this.ParseNumber(readType);
											return this.Value;
										}
										case '/':
										{
											this.ParseComment(false);
											goto Label0;
										}
									}
									break;
								}
							}
						}
						else if (chr > 'N')
						{
							if (chr == ']')
							{
								this._charPos++;
								if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
								{
									if (this._currentState != JsonReader.State.PostValue)
									{
										throw this.CreateUnexpectedCharacterException(chr);
									}
								}
								base.SetToken(JsonToken.EndArray);
								return null;
							}
							if (chr == 'n')
							{
								this.HandleNull();
								return null;
							}
						}
						else
						{
							if (chr == 'I')
							{
								return this.ParseNumberPositiveInfinity(readType);
							}
							if (chr == 'N')
							{
								return this.ParseNumberNaN(readType);
							}
						}
						this._charPos++;
						if (!char.IsWhiteSpace(chr))
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
						else
						{
							goto Label0;
						}
					}
					while (!this.ReadNullChar());
					base.SetToken(JsonToken.None, null, false);
					return null;
				}
				case JsonReader.State.Complete:
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
				case JsonReader.State.PostValue:
				{
					if (!this.ParsePostValue(true))
					{
						goto case JsonReader.State.Constructor;
					}
					return null;
				}
				case JsonReader.State.Finished:
				{
					this.ReadFinished();
					return null;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
		}

		private async Task<object> ReadNumberValueAsync(ReadType readType, CancellationToken cancellationToken)
		{
			JsonTextReader.<ReadNumberValueAsync>d__38 variable = new JsonTextReader.<ReadNumberValueAsync>d__38();
			variable.<>4__this = this;
			variable.readType = readType;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<ReadNumberValueAsync>d__38>(ref variable);
			return variable.<>t__builder.Task;
		}

		private void ReadStringIntoBuffer(char quote)
		{
			char chr;
			char chr1;
			bool flag;
			int num = this._charPos;
			int num1 = this._charPos;
			int num2 = this._charPos;
			this._stringBuffer.Position = 0;
			while (true)
			{
				int num3 = num;
				num = num3 + 1;
				char chr2 = this._chars[num3];
				if (chr2 > '\r')
				{
					if (chr2 != '\"' && chr2 != '\'')
					{
						if (chr2 == '\\')
						{
							this._charPos = num;
							if (!this.EnsureChars(0, true))
							{
								throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
							}
							int num4 = num - 1;
							chr = this._chars[num];
							num++;
							if (chr <= '\\')
							{
								if (chr > '\'')
								{
									if (chr == '/')
									{
										goto Label2;
									}
									if (chr == '\\')
									{
										chr1 = '\\';
										goto Label1;
									}
									else
									{
										break;
									}
								}
								else if (chr != '\"' && chr != '\'')
								{
									break;
								}
							Label2:
								chr1 = chr;
							}
							else if (chr > 'f')
							{
								if (chr == 'n')
								{
									chr1 = '\n';
								}
								else
								{
									switch (chr)
									{
										case 'r':
										{
											chr1 = '\r';
											break;
										}
										case 's':
										{
											this._charPos = num;
											throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Concat("\\", chr.ToString())));
										}
										case 't':
										{
											chr1 = '\t';
											break;
										}
										case 'u':
										{
											this._charPos = num;
											chr1 = this.ParseUnicode();
											if (StringUtils.IsLowSurrogate(chr1))
											{
												chr1 = '\uFFFD';
											}
											else if (StringUtils.IsHighSurrogate(chr1))
											{
												do
												{
													flag = false;
													if (this.EnsureChars(2, true))
													{
														if (this._chars[this._charPos] == '\\' && this._chars[this._charPos + 1] == 'u')
														{
															char chr3 = chr1;
															this._charPos += 2;
															chr1 = this.ParseUnicode();
															if (!StringUtils.IsLowSurrogate(chr1))
															{
																if (!StringUtils.IsHighSurrogate(chr1))
																{
																	chr3 = '\uFFFD';
																}
																else
																{
																	chr3 = '\uFFFD';
																	flag = true;
																}
															}
															this.EnsureBufferNotEmpty();
															this.WriteCharToBuffer(chr3, num2, num4);
															num2 = this._charPos;
															continue;
														}
													}
													chr1 = '\uFFFD';
												}
												while (flag);
											}
											num = this._charPos;
											break;
										}
										default:
										{
											this._charPos = num;
											throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Concat("\\", chr.ToString())));
										}
									}
								}
							}
							else if (chr == 'b')
							{
								chr1 = '\b';
							}
							else if (chr == 'f')
							{
								chr1 = '\f';
							}
							else
							{
								break;
							}
						Label1:
							this.EnsureBufferNotEmpty();
							this.WriteCharToBuffer(chr1, num2, num4);
							num2 = num;
						}
					}
					else if (this._chars[num - 1] == quote)
					{
						this.FinishReadStringIntoBuffer(num - 1, num1, num2);
						return;
					}
				}
				else if (chr2 == 0)
				{
					if (this._charsUsed == num - 1)
					{
						num--;
						if (this.ReadData(true) == 0)
						{
							this._charPos = num;
							throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
						}
					}
				}
				else if (chr2 == '\n')
				{
					this._charPos = num - 1;
					this.ProcessLineFeed();
					num = this._charPos;
				}
				else if (chr2 == '\r')
				{
					this._charPos = num - 1;
					this.ProcessCarriageReturn(true);
					num = this._charPos;
				}
			}
			this._charPos = num;
			throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Concat("\\", chr.ToString())));
		}

		private async Task ReadStringIntoBufferAsync(char quote, CancellationToken cancellationToken)
		{
			JsonTextReader.<ReadStringIntoBufferAsync>d__9 variable = new JsonTextReader.<ReadStringIntoBufferAsync>d__9();
			variable.<>4__this = this;
			variable.quote = quote;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<ReadStringIntoBufferAsync>d__9>(ref variable);
			return variable.<>t__builder.Task;
		}

		private object ReadStringValue(ReadType readType)
		{
			char chr;
			this.EnsureBuffer();
			switch (this._currentState)
			{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
				{
					while (true)
					{
						chr = this._chars[this._charPos];
						if (chr <= 'I')
						{
							if (chr > '\r')
							{
								switch (chr)
								{
									case ' ':
									{
										break;
									}
									case '!':
									case '#':
									case '$':
									case '%':
									case '&':
									case '(':
									case ')':
									case '*':
									case '+':
									{
										goto Label0;
									}
									case '\"':
									case '\'':
									{
										this.ParseString(chr, readType);
										return this.FinishReadQuotedStringValue(readType);
									}
									case ',':
									{
										this.ProcessValueComma();
										continue;
									}
									case '-':
									{
										if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
										{
											return this.ParseNumberNegativeInfinity(readType);
										}
										this.ParseNumber(readType);
										return this.Value;
									}
									case '.':
									case '0':
									case '1':
									case '2':
									case '3':
									case '4':
									case '5':
									case '6':
									case '7':
									case '8':
									case '9':
									{
										if (readType != ReadType.ReadAsString)
										{
											this._charPos++;
											throw this.CreateUnexpectedCharacterException(chr);
										}
										this.ParseNumber(ReadType.ReadAsString);
										return this.Value;
									}
									case '/':
									{
										this.ParseComment(false);
										continue;
									}
									default:
									{
										if (chr == 'I')
										{
											return this.ParseNumberPositiveInfinity(readType);
										}
										goto Label0;
									}
								}
							}
							else if (chr != 0)
							{
								switch (chr)
								{
									case '\t':
									{
										break;
									}
									case '\n':
									{
										this.ProcessLineFeed();
										continue;
									}
									case '\r':
									{
										this.ProcessCarriageReturn(false);
										continue;
									}
									default:
									{
										goto Label0;
									}
								}
							}
							else if (this.ReadNullChar())
							{
								base.SetToken(JsonToken.None, null, false);
								return null;
							}
							this._charPos++;
							continue;
						}
						else if (chr > ']')
						{
							if (chr == 'f')
							{
								break;
							}
							if (chr == 'n')
							{
								this.HandleNull();
								return null;
							}
							if (chr == 't')
							{
								break;
							}
						}
						else
						{
							if (chr == 'N')
							{
								return this.ParseNumberNaN(readType);
							}
							if (chr == ']')
							{
								this._charPos++;
								if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
								{
									if (this._currentState != JsonReader.State.PostValue)
									{
										throw this.CreateUnexpectedCharacterException(chr);
									}
								}
								base.SetToken(JsonToken.EndArray);
								return null;
							}
						}
					Label0:
						this._charPos++;
						if (!char.IsWhiteSpace(chr))
						{
							throw this.CreateUnexpectedCharacterException(chr);
						}
					}
					if (readType != ReadType.ReadAsString)
					{
						this._charPos++;
						throw this.CreateUnexpectedCharacterException(chr);
					}
					string str = (chr == 't' ? JsonConvert.True : JsonConvert.False);
					if (!this.MatchValueWithTrailingSeparator(str))
					{
						throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
					}
					base.SetToken(JsonToken.String, str);
					return str;
				}
				case JsonReader.State.Complete:
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
				case JsonReader.State.PostValue:
				{
					if (!this.ParsePostValue(true))
					{
						goto case JsonReader.State.Constructor;
					}
					return null;
				}
				case JsonReader.State.Finished:
				{
					this.ReadFinished();
					return null;
				}
				default:
				{
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
		}

		private async Task<object> ReadStringValueAsync(ReadType readType, CancellationToken cancellationToken)
		{
			JsonTextReader.<ReadStringValueAsync>d__37 variable = new JsonTextReader.<ReadStringValueAsync>d__37();
			variable.<>4__this = this;
			variable.readType = readType;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JsonTextReader.<ReadStringValueAsync>d__37>(ref variable);
			return variable.<>t__builder.Task;
		}

		private bool ReadUnquotedPropertyReportIfDone(char currentChar, int initialPosition)
		{
			if (this.ValidIdentifierChar(currentChar))
			{
				this._charPos++;
				return false;
			}
			if (!char.IsWhiteSpace(currentChar))
			{
				if (currentChar != ':')
				{
					throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
				}
			}
			this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
			return true;
		}

		private void SetNewLine(bool hasNextChar)
		{
			if (hasNextChar && this._chars[this._charPos] == '\n')
			{
				this._charPos++;
			}
			this.OnNewLine(this._charPos);
		}

		private void ShiftBufferIfNeeded()
		{
			int length = (int)this._chars.Length;
			if ((double)(length - this._charPos) <= (double)length * 0.1 || length >= 1073741823)
			{
				int num = this._charsUsed - this._charPos;
				if (num > 0)
				{
					JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num);
				}
				this._lineStartPos -= this._charPos;
				this._charPos = 0;
				this._charsUsed = num;
				this._chars[this._charsUsed] = '\0';
			}
		}

		private JsonReaderException ThrowReaderError(string message, Exception ex = null)
		{
			base.SetToken(JsonToken.Undefined, null, false);
			return JsonReaderException.Create(this, message, ex);
		}

		private bool ValidIdentifierChar(char value)
		{
			if (char.IsLetterOrDigit(value) || value == '\u005F')
			{
				return true;
			}
			return value == '$';
		}

		public override int? vmethod_0()
		{
			return (int?)this.ReadNumberValue(ReadType.const_1);
		}

		private void WriteCharToBuffer(char writeChar, int lastWritePosition, int writeToPosition)
		{
			if (writeToPosition > lastWritePosition)
			{
				this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, writeToPosition - lastWritePosition);
			}
			this._stringBuffer.Append(this._arrayPool, writeChar);
		}
	}
}