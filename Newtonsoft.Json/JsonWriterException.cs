using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	[Serializable]
	public class JsonWriterException : JsonException
	{
		public string Path
		{
			get;
		}

		public JsonWriterException()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonWriterException(string message)
		{
			Class6.yDnXvgqzyB5jw();
			base(message);
		}

		public JsonWriterException(string message, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
		}

		public JsonWriterException(SerializationInfo info, StreamingContext context)
		{
			Class6.yDnXvgqzyB5jw();
			base(info, context);
		}

		public JsonWriterException(string message, string path, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
			this.Path = path;
		}

		internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		internal static JsonWriterException Create(string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, path, ex);
		}
	}
}