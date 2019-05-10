using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		public int LineNumber
		{
			get;
		}

		public int LinePosition
		{
			get;
		}

		public string Path
		{
			get;
		}

		public JsonSerializationException()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonSerializationException(string message)
		{
			Class6.yDnXvgqzyB5jw();
			base(message);
		}

		public JsonSerializationException(string message, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
		}

		public JsonSerializationException(SerializationInfo info, StreamingContext context)
		{
			Class6.yDnXvgqzyB5jw();
			base(info, context);
		}

		public JsonSerializationException(string message, string path, int lineNumber, int linePosition, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return JsonSerializationException.Create(reader, message, null);
		}

		internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			int lineNumber;
			int linePosition;
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			if (lineInfo == null || !lineInfo.HasLineInfo())
			{
				lineNumber = 0;
				linePosition = 0;
			}
			else
			{
				lineNumber = lineInfo.LineNumber;
				linePosition = lineInfo.LinePosition;
			}
			return new JsonSerializationException(message, path, lineNumber, linePosition, ex);
		}
	}
}