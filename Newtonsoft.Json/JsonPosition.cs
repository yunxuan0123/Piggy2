using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Newtonsoft.Json
{
	internal struct JsonPosition
	{
		private readonly static char[] SpecialCharacters;

		internal JsonContainerType Type;

		internal int Position;

		internal string PropertyName;

		internal bool HasIndex;

		static JsonPosition()
		{
			Class6.yDnXvgqzyB5jw();
			JsonPosition.SpecialCharacters = new char[] { '.', ' ', '\'', '/', '\"', '[', ']', '(', ')', '\t', '\n', '\r', '\f', '\b', '\\', '\u0085', '\u2028', '\u2029' };
		}

		public JsonPosition(JsonContainerType type)
		{
			Class6.yDnXvgqzyB5jw();
			this.Type = type;
			this.HasIndex = JsonPosition.TypeHasIndex(type);
			this.Position = -1;
			this.PropertyName = null;
		}

		internal static string BuildPath(List<JsonPosition> positions, JsonPosition? currentPosition)
		{
			JsonPosition item;
			int num = 0;
			if (positions != null)
			{
				for (int i = 0; i < positions.Count; i++)
				{
					item = positions[i];
					num += item.CalculateLength();
				}
			}
			if (currentPosition.HasValue)
			{
				item = currentPosition.GetValueOrDefault();
				num += item.CalculateLength();
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			StringWriter stringWriter = null;
			char[] chrArray = null;
			if (positions != null)
			{
				foreach (JsonPosition position in positions)
				{
					position.WriteTo(stringBuilder, ref stringWriter, ref chrArray);
				}
			}
			if (currentPosition.HasValue)
			{
				item = currentPosition.GetValueOrDefault();
				item.WriteTo(stringBuilder, ref stringWriter, ref chrArray);
			}
			return stringBuilder.ToString();
		}

		internal int CalculateLength()
		{
			JsonContainerType type = this.Type;
			if (type == JsonContainerType.Object)
			{
				return this.PropertyName.Length + 5;
			}
			if ((int)type - (int)JsonContainerType.Array > (int)JsonContainerType.Object)
			{
				throw new ArgumentOutOfRangeException("Type");
			}
			return MathUtils.IntLength((ulong)this.Position) + 2;
		}

		internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
		{
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
			{
				message = message.Trim();
				if (!message.EndsWith('.'))
				{
					message = string.Concat(message, ".");
				}
				message = string.Concat(message, " ");
			}
			message = string.Concat(message, "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path));
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				message = string.Concat(message, ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition));
			}
			message = string.Concat(message, ".");
			return message;
		}

		internal static bool TypeHasIndex(JsonContainerType type)
		{
			if (type == JsonContainerType.Array)
			{
				return true;
			}
			return type == JsonContainerType.Constructor;
		}

		internal void WriteTo(StringBuilder sb, ref StringWriter writer, ref char[] buffer)
		{
			JsonContainerType type = this.Type;
			if (type != JsonContainerType.Object)
			{
				if ((int)type - (int)JsonContainerType.Array > (int)JsonContainerType.Object)
				{
					return;
				}
				sb.Append('[');
				sb.Append(this.Position);
				sb.Append(']');
				return;
			}
			string propertyName = this.PropertyName;
			if (propertyName.IndexOfAny(JsonPosition.SpecialCharacters) == -1)
			{
				if (sb.Length > 0)
				{
					sb.Append('.');
				}
				sb.Append(propertyName);
				return;
			}
			sb.Append("['");
			if (writer == null)
			{
				writer = new StringWriter(sb);
			}
			JavaScriptUtils.WriteEscapedJavaScriptString(writer, propertyName, '\'', false, JavaScriptUtils.SingleQuoteCharEscapeFlags, StringEscapeHandling.Default, null, ref buffer);
			sb.Append("']");
		}
	}
}