using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	[Serializable]
	public class JsonException : Exception
	{
		public JsonException()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonException(string message)
		{
			Class6.yDnXvgqzyB5jw();
			base(message);
		}

		public JsonException(string message, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
		}

		public JsonException(SerializationInfo info, StreamingContext context)
		{
			Class6.yDnXvgqzyB5jw();
			base(info, context);
		}

		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}