using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	[Serializable]
	public class JsonReaderException : JsonException
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

		public JsonReaderException()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonReaderException(string message)
		{
			Class6.yDnXvgqzyB5jw();
			base(message);
		}

		public JsonReaderException(string message, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
		}

		public JsonReaderException(SerializationInfo info, StreamingContext context)
		{
			Class6.yDnXvgqzyB5jw();
			base(info, context);
		}

		public JsonReaderException(string message, string path, int lineNumber, int linePosition, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return JsonReaderException.Create(reader, message, null);
		}

		internal static JsonReaderException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		internal static JsonReaderException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
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
			return new JsonReaderException(message, path, lineNumber, linePosition, ex);
		}
	}
}