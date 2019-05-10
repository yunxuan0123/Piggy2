using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
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

		public JsonSchemaException()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonSchemaException(string message)
		{
			Class6.yDnXvgqzyB5jw();
			base(message);
		}

		public JsonSchemaException(string message, Exception innerException)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
		}

		public JsonSchemaException(SerializationInfo info, StreamingContext context)
		{
			Class6.yDnXvgqzyB5jw();
			base(info, context);
		}

		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
		{
			Class6.yDnXvgqzyB5jw();
			base(message, innerException);
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}