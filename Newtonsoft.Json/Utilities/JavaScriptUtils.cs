using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	internal static class JavaScriptUtils
	{
		internal readonly static bool[] SingleQuoteCharEscapeFlags;

		internal readonly static bool[] DoubleQuoteCharEscapeFlags;

		internal readonly static bool[] HtmlCharEscapeFlags;

		static JavaScriptUtils()
		{
			Class6.yDnXvgqzyB5jw();
			JavaScriptUtils.SingleQuoteCharEscapeFlags = new bool[128];
			JavaScriptUtils.DoubleQuoteCharEscapeFlags = new bool[128];
			JavaScriptUtils.HtmlCharEscapeFlags = new bool[128];
			IList<char> chrs = new List<char>()
			{
				'\n',
				'\r',
				'\t',
				'\\',
				'\f',
				'\b'
			};
			for (int i = 0; i < 32; i++)
			{
				chrs.Add((char)i);
			}
			foreach (char chr in chrs.Union<char>((IEnumerable<char>)(new char[] { '\'' })))
			{
				JavaScriptUtils.SingleQuoteCharEscapeFlags[chr] = true;
			}
			foreach (char chr1 in chrs.Union<char>((IEnumerable<char>)(new char[] { '\"' })))
			{
				JavaScriptUtils.DoubleQuoteCharEscapeFlags[chr1] = true;
			}
			foreach (char chr2 in chrs.Union<char>((IEnumerable<char>)(new char[] { '\"', '\'', '<', '>', '&' })))
			{
				JavaScriptUtils.HtmlCharEscapeFlags[chr2] = true;
			}
		}

		private static int FirstCharToEscape(string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling)
		{
			int num = 0;
			while (true)
			{
				if (num == s.Length)
				{
					return -1;
				}
				char chr = s[num];
				if (chr >= (char)((int)charEscapeFlags.Length))
				{
					if (stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
					{
						return num;
					}
					if (chr == '\u0085' || chr == '\u2028')
					{
						break;
					}
					if (chr == '\u2029')
					{
						break;
					}
				}
				else if (charEscapeFlags[chr])
				{
					return num;
				}
				num++;
			}
			return num;
		}

		public static bool[] GetCharEscapeFlags(StringEscapeHandling stringEscapeHandling, char quoteChar)
		{
			if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
			{
				return JavaScriptUtils.HtmlCharEscapeFlags;
			}
			if (quoteChar == '\"')
			{
				return JavaScriptUtils.DoubleQuoteCharEscapeFlags;
			}
			return JavaScriptUtils.SingleQuoteCharEscapeFlags;
		}

		public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
		{
			if (s == null)
			{
				return false;
			}
			string str = s;
			for (int i = 0; i < str.Length; i++)
			{
				char chr = str[i];
				if (chr >= (char)((int)charEscapeFlags.Length) || charEscapeFlags[chr])
				{
					return true;
				}
			}
			return false;
		}

		public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters, StringEscapeHandling stringEscapeHandling)
		{
			string str;
			bool[] charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(stringEscapeHandling, delimiter);
			using (StringWriter stringWriter = StringUtils.CreateStringWriter((value != null ? value.Length : 16)))
			{
				char[] chrArray = null;
				JavaScriptUtils.WriteEscapedJavaScriptString(stringWriter, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, null, ref chrArray);
				str = stringWriter.ToString();
			}
			return str;
		}

		private static bool TryGetDateConstructorValue(JsonReader reader, out long? integer, out string errorMessage)
		{
			integer = null;
			errorMessage = null;
			if (!reader.Read())
			{
				errorMessage = "Unexpected end when reading date constructor.";
				return false;
			}
			if (reader.TokenType == JsonToken.EndConstructor)
			{
				return true;
			}
			if (reader.TokenType == JsonToken.Integer)
			{
				integer = new long?((long)reader.Value);
				return true;
			}
			errorMessage = string.Concat("Unexpected token when reading date constructor. Expected Integer, got ", reader.TokenType);
			return false;
		}

		public static bool TryGetDateFromConstructorJson(JsonReader reader, out DateTime dateTime, out string errorMessage)
		{
			long? nullable;
			long? nullable1;
			long? nullable2;
			dateTime = new DateTime();
			errorMessage = null;
			if (!JavaScriptUtils.TryGetDateConstructorValue(reader, out nullable, out errorMessage) || !nullable.HasValue)
			{
				errorMessage = errorMessage ?? "Date constructor has no arguments.";
				return false;
			}
			if (!JavaScriptUtils.TryGetDateConstructorValue(reader, out nullable1, out errorMessage))
			{
				return false;
			}
			if (!nullable1.HasValue)
			{
				dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(nullable.Value);
			}
			else
			{
				List<long> nums = new List<long>()
				{
					nullable.Value,
					nullable1.Value
				};
				while (JavaScriptUtils.TryGetDateConstructorValue(reader, out nullable2, out errorMessage))
				{
					if (!nullable2.HasValue)
					{
						if (nums.Count > 7)
						{
							errorMessage = "Unexpected number of arguments when reading date constructor.";
							return false;
						}
						while (nums.Count < 7)
						{
							nums.Add(0L);
						}
						dateTime = new DateTime((int)nums[0], (int)nums[1] + 1, (nums[2] == 0 ? 1 : (int)nums[2]), (int)nums[3], (int)nums[4], (int)nums[5], (int)nums[6]);
						return true;
					}
					else
					{
						nums.Add(nullable2.Value);
					}
				}
				return false;
			}
			return true;
		}

		public static async Task WriteCharAsync(Task task, TextWriter writer, char c, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = writer.WriteAsync(c, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		private static async Task WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(TextWriter writer, string s, int lastWritePosition, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			int length;
			ConfiguredTaskAwaitable configuredTaskAwaitable;
			char chr;
			if (writeBuffer == null || (int)writeBuffer.Length < lastWritePosition)
			{
				writeBuffer = client.EnsureWriteBuffer(lastWritePosition, 6);
			}
			if (lastWritePosition != 0)
			{
				s.CopyTo(0, writeBuffer, 0, lastWritePosition);
				configuredTaskAwaitable = writer.WriteAsync(writeBuffer, 0, lastWritePosition, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			bool flag = false;
			string str = null;
			for (int i = lastWritePosition; i < s.Length; i++)
			{
				chr = s[i];
				if (chr >= (char)((int)charEscapeFlags.Length) || charEscapeFlags[chr])
				{
					if (chr <= '\\')
					{
						switch (chr)
						{
							case '\b':
							{
								str = "\\b";
								break;
							}
							case '\t':
							{
								str = "\\t";
								break;
							}
							case '\n':
							{
								str = "\\n";
								break;
							}
							case '\v':
							{
								goto Label0;
							}
							case '\f':
							{
								str = "\\f";
								break;
							}
							case '\r':
							{
								str = "\\r";
								break;
							}
							default:
							{
								if (chr == '\\')
								{
									str = "\\\\";
									break;
								}
								else
								{
									goto Label0;
								}
							}
						}
					}
					else if (chr == '\u0085')
					{
						str = "\\u0085";
					}
					else if (chr == '\u2028')
					{
						str = "\\u2028";
					}
					else
					{
						if (chr != '\u2029')
						{
							goto Label0;
						}
						str = "\\u2029";
					}
				Label2:
					if (i > lastWritePosition)
					{
						int num = i - lastWritePosition;
						length = num + (flag ? 6 : 0);
						int num1 = (flag ? 6 : 0);
						if ((int)writeBuffer.Length < length)
						{
							writeBuffer = client.EnsureWriteBuffer(length, 6);
						}
						s.CopyTo(lastWritePosition, writeBuffer, num1, length - num1);
						configuredTaskAwaitable = writer.WriteAsync(writeBuffer, num1, length - num1, cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable;
					}
					lastWritePosition = i + 1;
					if (flag)
					{
						configuredTaskAwaitable = writer.WriteAsync(writeBuffer, 0, 6, cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable;
						flag = false;
					}
					else
					{
						configuredTaskAwaitable = writer.WriteAsync(str, cancellationToken).ConfigureAwait(false);
						await configuredTaskAwaitable;
					}
				}
			Label1:
			}
			length = s.Length - lastWritePosition;
			if (length != 0)
			{
				if ((int)writeBuffer.Length < length)
				{
					writeBuffer = client.EnsureWriteBuffer(length, 0);
				}
				s.CopyTo(lastWritePosition, writeBuffer, 0, length);
				configuredTaskAwaitable = writer.WriteAsync(writeBuffer, 0, length, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			return;
		Label0:
			if (chr >= (char)((int)charEscapeFlags.Length) && stringEscapeHandling != StringEscapeHandling.EscapeNonAscii)
			{
				goto Label1;
			}
			if (chr == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
			{
				str = "\\'";
				goto Label2;
			}
			else if (chr != '\"' || stringEscapeHandling == StringEscapeHandling.EscapeHtml)
			{
				if ((int)writeBuffer.Length < 6)
				{
					writeBuffer = client.EnsureWriteBuffer(6, 0);
				}
				StringUtils.ToCharAsUnicode(chr, writeBuffer);
				flag = true;
				goto Label2;
			}
			else
			{
				str = "\\\"";
				goto Label2;
			}
		}

		public static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter, bool appendDelimiters, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, IArrayPool<char> bufferPool, ref char[] writeBuffer)
		{
			int length;
			char chr;
			string str;
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			if (!string.IsNullOrEmpty(s))
			{
				int escape = JavaScriptUtils.FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
				if (escape != -1)
				{
					if (escape != 0)
					{
						if (writeBuffer == null || (int)writeBuffer.Length < escape)
						{
							writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, escape, writeBuffer);
						}
						s.CopyTo(0, writeBuffer, 0, escape);
						writer.Write(writeBuffer, 0, escape);
					}
					for (int i = escape; i < s.Length; i++)
					{
						chr = s[i];
						if (chr >= (char)((int)charEscapeFlags.Length) || charEscapeFlags[chr])
						{
							if (chr <= '\\')
							{
								switch (chr)
								{
									case '\b':
									{
										str = "\\b";
										break;
									}
									case '\t':
									{
										str = "\\t";
										break;
									}
									case '\n':
									{
										str = "\\n";
										break;
									}
									case '\v':
									{
										goto Label0;
									}
									case '\f':
									{
										str = "\\f";
										break;
									}
									case '\r':
									{
										str = "\\r";
										break;
									}
									default:
									{
										if (chr == '\\')
										{
											str = "\\\\";
											break;
										}
										else
										{
											goto Label0;
										}
									}
								}
							}
							else if (chr == '\u0085')
							{
								str = "\\u0085";
							}
							else if (chr == '\u2028')
							{
								str = "\\u2028";
							}
							else
							{
								if (chr != '\u2029')
								{
									goto Label0;
								}
								str = "\\u2029";
							}
						Label1:
							if (str != null)
							{
								bool flag = string.Equals(str, "!");
								if (i > escape)
								{
									length = i - escape + (flag ? 6 : 0);
									int num = (flag ? 6 : 0);
									if (writeBuffer == null || (int)writeBuffer.Length < length)
									{
										char[] chrArray = BufferUtils.RentBuffer(bufferPool, length);
										if (flag)
										{
											Array.Copy(writeBuffer, chrArray, 6);
										}
										BufferUtils.ReturnBuffer(bufferPool, writeBuffer);
										writeBuffer = chrArray;
									}
									s.CopyTo(escape, writeBuffer, num, length - num);
									writer.Write(writeBuffer, num, length - num);
								}
								escape = i + 1;
								if (flag)
								{
									writer.Write(writeBuffer, 0, 6);
								}
								else
								{
									writer.Write(str);
								}
							}
						}
					}
					length = s.Length - escape;
					if (length > 0)
					{
						if (writeBuffer == null || (int)writeBuffer.Length < length)
						{
							writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, length, writeBuffer);
						}
						s.CopyTo(escape, writeBuffer, 0, length);
						writer.Write(writeBuffer, 0, length);
					}
				}
				else
				{
					writer.Write(s);
				}
			}
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			return;
		Label0:
			if (chr >= (char)((int)charEscapeFlags.Length))
			{
				if (stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
				{
					goto Label3;
				}
				str = null;
				goto Label1;
			}
			if (chr == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
			{
				str = "\\'";
				goto Label1;
			}
			else if (chr != '\"' || stringEscapeHandling == StringEscapeHandling.EscapeHtml)
			{
				if (writeBuffer == null || (int)writeBuffer.Length < 6)
				{
					writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, 6, writeBuffer);
				}
				StringUtils.ToCharAsUnicode(chr, writeBuffer);
				str = "!";
				goto Label1;
			}
			else
			{
				str = "\\\"";
				goto Label1;
			}
		}

		public static Task WriteEscapedJavaScriptStringAsync(TextWriter writer, string s, char delimiter, bool appendDelimiters, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken = null)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			if (appendDelimiters)
			{
				return JavaScriptUtils.WriteEscapedJavaScriptStringWithDelimitersAsync(writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			if (!string.IsNullOrEmpty(s))
			{
				return JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			return cancellationToken.CancelIfRequestedAsync() ?? AsyncUtils.CompletedTask;
		}

		private static Task WriteEscapedJavaScriptStringWithDelimitersAsync(TextWriter writer, string s, char delimiter, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			Task task = writer.WriteAsync(delimiter, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return JavaScriptUtils.WriteEscapedJavaScriptStringWithDelimitersAsync(task, writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			if (!string.IsNullOrEmpty(s))
			{
				task = JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
				if (task.IsCompletedSucessfully())
				{
					return writer.WriteAsync(delimiter, cancellationToken);
				}
			}
			return JavaScriptUtils.WriteCharAsync(task, writer, delimiter, cancellationToken);
		}

		private static async Task WriteEscapedJavaScriptStringWithDelimitersAsync(Task task, TextWriter writer, string s, char delimiter, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (!string.IsNullOrEmpty(s))
			{
				configuredTaskAwaitable = JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			configuredTaskAwaitable = writer.WriteAsync(delimiter).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		private static Task WriteEscapedJavaScriptStringWithoutDelimitersAsync(TextWriter writer, string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			int escape = JavaScriptUtils.FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
			if (escape == -1)
			{
				return writer.WriteAsync(s, cancellationToken);
			}
			return JavaScriptUtils.WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, escape, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
		}
	}
}